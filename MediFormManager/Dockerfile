# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /source

COPY src/ ./src/

RUN dotnet restore src/MediFormManager.Api/MediFormManager.Api.csproj

RUN dotnet publish src/MediFormManager.Api/MediFormManager.Api.csproj \
    -c Release \
    --no-restore \
    -o /app/publish

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "MediFormManager.Api.dll"]