FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/VirtualParkApi
RUN dotnet restore "VirtualParkApi.csproj" --disable-parallel

RUN dotnet publish "VirtualParkApi.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app ./

RUN mkdir -p /app/plugins

EXPOSE 5222
ENTRYPOINT ["dotnet", "VirtualParkApi.dll"]