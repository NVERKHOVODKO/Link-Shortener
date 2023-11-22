FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["link shortener/link shortener.csproj", "link shortener/"]
RUN dotnet restore "link shortener/link shortener.csproj"
COPY . .
WORKDIR "/src/link shortener"
RUN dotnet build "link shortener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "link shortener.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "link shortener.dll"]
