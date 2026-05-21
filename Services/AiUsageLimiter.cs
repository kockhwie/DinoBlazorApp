using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace DinoBlazorApp.Services;

public interface IAiUsageLimiter
{
    ValueTask<AiUsageLease?> TryAcquireAsync(string usageKey, CancellationToken cancellationToken);
}

public sealed class AiUsageLimiter : IAiUsageLimiter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly TimeSpan SessionWindow = TimeSpan.FromHours(1);
    private static readonly TimeSpan GlobalWindow = TimeSpan.FromMinutes(1);
    private const int MaxSessionRequestsPerWindow = 20;
    private const int MaxGlobalRequestsPerWindow = 60;
    private const int MaxConcurrentRequests = 4;

    private readonly object _gate = new();
    private readonly ConcurrentDictionary<string, Queue<DateTimeOffset>> _sessionRequests = new(StringComparer.Ordinal);
    private readonly Queue<DateTimeOffset> _globalRequests = new();
    private readonly SemaphoreSlim _concurrency = new(MaxConcurrentRequests, MaxConcurrentRequests);

    public AiUsageLimiter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async ValueTask<AiUsageLease?> TryAcquireAsync(string usageKey, CancellationToken cancellationToken)
    {
        var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var sessionKey = string.IsNullOrWhiteSpace(usageKey) ? "anonymous" : usageKey;
        var normalizedKey = $"{sessionKey}:{clientIp}";

        lock (_gate)
        {
            var now = DateTimeOffset.UtcNow;
            TrimQueue(_globalRequests, now - GlobalWindow);

            if (_globalRequests.Count >= MaxGlobalRequestsPerWindow)
            {
                return AiUsageLease.Rejected("The app is busy right now. Please wait a minute and try again.");
            }

            var sessionQueue = _sessionRequests.GetOrAdd(normalizedKey, _ => new Queue<DateTimeOffset>());
            TrimQueue(sessionQueue, now - SessionWindow);

            if (sessionQueue.Count >= MaxSessionRequestsPerWindow)
            {
                return AiUsageLease.Rejected("You've reached the hourly AI request limit. Please try again later.");
            }

            _globalRequests.Enqueue(now);
            sessionQueue.Enqueue(now);
        }

        if (!await _concurrency.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken))
        {
            return AiUsageLease.Rejected("Too many AI responses are running right now. Please try again in a moment.");
        }

        return AiUsageLease.Accepted(_concurrency);
    }

    private static void TrimQueue(Queue<DateTimeOffset> queue, DateTimeOffset cutoff)
    {
        while (queue.TryPeek(out var seenAt) && seenAt < cutoff)
        {
            queue.Dequeue();
        }
    }
}

public sealed class AiUsageLease : IAsyncDisposable
{
    private readonly SemaphoreSlim? _concurrency;

    private AiUsageLease(bool isAccepted, string? rejectionReason, SemaphoreSlim? concurrency)
    {
        IsAccepted = isAccepted;
        RejectionReason = rejectionReason;
        _concurrency = concurrency;
    }

    public bool IsAccepted { get; }
    public string? RejectionReason { get; }

    public static AiUsageLease Accepted(SemaphoreSlim concurrency) => new(true, null, concurrency);
    public static AiUsageLease Rejected(string reason) => new(false, reason, null);

    public ValueTask DisposeAsync()
    {
        _concurrency?.Release();
        return ValueTask.CompletedTask;
    }
}
