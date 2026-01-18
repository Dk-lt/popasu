# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY popasu.sln ./
COPY popasu.Api/popasu.Api.csproj popasu.Api/
COPY Client/popasu.Client.csproj Client/

COPY . .

WORKDIR /src/popasu.Api
RUN dotnet restore ../popasu.sln && \
    dotnet publish -c Release -o /app/publish

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "popasu.Api.dll"]
