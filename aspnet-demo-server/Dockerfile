FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["aspnet-demo-server/aspnet-demo-server.csproj", "aspnet-demo-server/"]
RUN dotnet restore "aspnet-demo-server/aspnet-demo-server.csproj"
COPY . .
WORKDIR "/src/aspnet-demo-server"
RUN dotnet build "aspnet-demo-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "aspnet-demo-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnet-demo-server.dll"]
