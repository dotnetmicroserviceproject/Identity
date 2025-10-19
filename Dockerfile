# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

# Creates a non-root user for security
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["src/User.Contracts/User.Contracts.csproj", "src/User.Contracts/"]
COPY ["src/User.Service/User.Service.csproj", "src/User.Service/"]

# Add GitHub NuGet source using Docker secrets
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER \
    --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    dotnet nuget add source \
    --username USERNAME \
    --password "$(cat /GH_PAT)" \
    --store-password-in-clear-text \
    --name github "https://nuget.pkg.github.com/$(cat /GH_OWNER)/index.json"

# Restore packages
RUN dotnet restore "src/User.Service/User.Service.csproj"

# Copy the rest of the source code
COPY ./src ./src

# Publish the app
WORKDIR "/src/User.Service"
RUN dotnet publish "User.Service.csproj" -c Release --no-restore -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "User.Service.dll"]
