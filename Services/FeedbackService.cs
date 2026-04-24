using System.Text.Json;
using DinoBlazorApp.Models;

namespace DinoBlazorApp.Services;

public interface IFeedbackService
{
    List<FeedbackEntry> GetFeedback();
    List<WishlistItem> GetWishlist();
    void AddFeedback(FeedbackEntry entry);
    void AddWishlistItem(WishlistItem item);
    void ToggleVote(int itemId);
}

public class FeedbackService : IFeedbackService
{
    private readonly string _storePath;
    private FeedbackDataStore _store = new();

    public FeedbackService()
    {
        var appDataFolder = Path.Combine(Environment.CurrentDirectory, ".appdata");
        if (!Directory.Exists(appDataFolder))
        {
            Directory.CreateDirectory(appDataFolder);
        }
        _storePath = Path.Combine(appDataFolder, "feedback_store.json");
        LoadStore();
    }

    private void LoadStore()
    {
        if (File.Exists(_storePath))
        {
            try
            {
                var json = File.ReadAllText(_storePath);
                _store = JsonSerializer.Deserialize<FeedbackDataStore>(json) ?? new FeedbackDataStore();
                return;
            }
            catch
            {
                // Fallback on corrupt JSON
            }
        }
        
        _store = new FeedbackDataStore();
        // Seed defaults if empty
        _store.WishlistItems.AddRange(new[]
        {
            new WishlistItem { Id=1, Title="Save favourite prompts",          Category="ux",      Desc="Let me bookmark prompts I use often so I don't have to retype them.",     Votes=24, IsNew=false },
            new WishlistItem { Id=2, Title="Prompt history across sessions",  Category="feature", Desc="See my past conversations even after refreshing the page.",                Votes=19, IsNew=false },
            new WishlistItem { Id=3, Title="Dark / light theme toggle",       Category="design",  Desc="Some of us prefer a lighter interface during the day.",                   Votes=15, IsNew=false },
            new WishlistItem { Id=4, Title="Export response as PDF or Word",  Category="feature", Desc="One-click export so I can share AI responses with colleagues easily.",     Votes=13, IsNew=false },
            new WishlistItem { Id=5, Title="More languages in translation",   Category="feature", Desc="Arabic, Hindi, Thai, Korean, Portuguese would cover more of our users.",   Votes=11, IsNew=false },
            new WishlistItem { Id=6, Title="User accounts & login",           Category="feature", Desc="So my progress and saved prompts persist across devices.",                 Votes=10, IsNew=false },
            new WishlistItem { Id=7, Title="Prompt scoring after each reply", Category="learn",   Desc="Show me a score for my prompt so I know how to improve.",                  Votes=9,  IsNew=false },
            new WishlistItem { Id=8, Title="Side-by-side prompt comparison",  Category="learn",   Desc="Run two versions of the same prompt and compare the outputs.",             Votes=7,  IsNew=false },
        });
        SaveStore();
    }

    private void SaveStore()
    {
        var json = JsonSerializer.Serialize(_store, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_storePath, json);
    }

    public List<FeedbackEntry> GetFeedback() => _store.RecentFeedback;
    
    public List<WishlistItem> GetWishlist() => _store.WishlistItems;

    public void AddFeedback(FeedbackEntry entry)
    {
        _store.RecentFeedback.Add(entry);
        SaveStore();
    }

    public void AddWishlistItem(WishlistItem item)
    {
        item.Id = _store.WishlistItems.Count > 0 ? _store.WishlistItems.Max(i => i.Id) + 1 : 1;
        _store.WishlistItems.Insert(0, item);
        SaveStore();
    }

    public void ToggleVote(int itemId)
    {
        var item = _store.WishlistItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            if (item.Voted) { item.Votes--; item.Voted = false; }
            else { item.Votes++; item.Voted = true; }
            SaveStore();
        }
    }
}
