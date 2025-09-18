FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0.102 AS build
WORKDIR /src
COPY . .
RUN dotnet publish MyBooksandMovies/MyBooksandMovies.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyBooksandMovies.dll"]
