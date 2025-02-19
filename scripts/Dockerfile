FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Leaderboard.sln", "./"]
COPY ["src/Leaderboard.Api/Leaderboard.Api.csproj", "src/Leaderboard.Api/"]
COPY ["src/Leaderboard.Application/Leaderboard.Application.csproj", "src/Leaderboard.Application/"]
COPY ["src/Leaderboard.Core/Leaderboard.Core.csproj", "src/Leaderboard.Core/"]
COPY ["src/Leaderboard.Infrastructure/Leaderboard.Infrastructure.csproj", "src/Leaderboard.Infrastructure/"]

RUN dotnet restore

COPY . .

WORKDIR "/src/src/Leaderboard.Api"
RUN dotnet publish "Leaderboard.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5005
ENTRYPOINT ["dotnet", "Leaderboard.Api.dll"]
