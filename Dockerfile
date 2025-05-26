# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./Backend ./Backend
WORKDIR /src/Backend
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

Expose 80
Expose 433

ENTRYPOINT ["dotnet", "Backend.dll"]
