# -----------------------------
# Base image for running the app
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# -----------------------------
# Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["src/User.Contracts/User.Contracts.csproj", "src/User.Contracts/"]
COPY ["src/User.Service/User.Service.csproj", "src/User.Service/"]

# Authenticate to GitHub Packages using secrets and restore
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER \
    --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    bash -c '\
        GH_OWNER=$(cat /GH_OWNER) && \
        GH_PAT=$(cat /GH_PAT) && \
        echo "machine nuget.pkg.github.com login ${GH_OWNER} password ${GH_PAT}" > ~/.netrc && \
        dotnet nuget add source \
            --username ${GH_OWNER} \
            --password ${GH_PAT} \
            --store-password-in-clear-text \
            --name github "https://nuget.pkg.github.com/${GH_OWNER}/index.json" && \
        dotnet restore "src/User.Service/User.Service.csproj" --disable-parallel --ignore-failed-sources --verbosity minimal \
    '

# Copy all source files
COPY ./src ./src

# ✅ Fix: Adjusted working directory to match your actual folder structure inside container
WORKDIR "/src/src/User.Service"

# Publish the application
RUN dotnet publish "User.Service.csproj" -c Release --no-restore -o /app/publish


# -----------------------------
# Final runtime image
# -----------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "User.Service.dll"]
