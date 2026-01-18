# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY popasu.sln ./
COPY Client/popasu.Client.csproj Client/
COPY popasu.Api/popasu.Api.csproj popasu.Api/

COPY . .

WORKDIR /src/Client
RUN dotnet restore ../popasu.sln && \
    dotnet publish -c Release -o /app/publish

# ===== serve static =====
FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

COPY Client/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/publish/wwwroot ./

EXPOSE 80
