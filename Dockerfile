# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file and restore first to leverage layer caching and avoid copying everything
COPY ["DinoBlazorApp-v2.csproj", "./"]
RUN dotnet restore "DinoBlazorApp-v2.csproj"

# Copy the remaining sources and publish
COPY . .
RUN dotnet publish "DinoBlazorApp-v2.csproj" -c Release -o /app --no-restore

# Runtime stage (use a non-root user)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create a non-root user and group and give ownership of /app to that user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Copy published output and set ownership
COPY --from=build /app ./
RUN chown -R appuser:appgroup /app

# Run as non-root user for improved container security
USER appuser

ENTRYPOINT ["dotnet", "DinoBlazorApp-v2.dll"]
