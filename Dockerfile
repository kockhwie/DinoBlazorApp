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

# Copy published output
COPY --from=build /app ./

# Create a non-root ownership and avoid adduser/addgroup which may not exist in minimal images.
# Use numeric UID 1000 (common non-root user) so the container runs without root privileges.
RUN mkdir -p /home/app && chown -R 1000:1000 /app /home/app

# Run as non-root numeric user
USER 1000

ENTRYPOINT ["dotnet", "DinoBlazorApp-v2.dll"]
