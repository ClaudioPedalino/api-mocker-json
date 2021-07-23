#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["mock-json/mock-json.csproj", "mock-json/"]
RUN dotnet restore "mock-json/mock-json.csproj"
COPY . .
WORKDIR "/src/mock-json"
RUN dotnet build "mock-json.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mock-json.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mock-json.dll"]