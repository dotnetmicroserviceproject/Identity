FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

# Creates a non-root user with an explicit UID and adds permission to access the /app folder 
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers 
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app 
USER appuser 
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build 
COPY ["src/User.Contracts/User.Contracts.csproj", "src/User.Contracts/"] 
COPY ["src/User.Service/User.Service.csproj", "src/User.Service/"] 
RUN dotnet restore "src/User.Service/User.Service.csproj" 
COPY ./src ./src 
WORKDIR "/src/User.Service" 
RUN dotnet publish "User.Service.csproj" -c Release --no-restore -o /app/publish 
FROM base AS final 
WORKDIR /app 
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet", "User.Service.dll"]
