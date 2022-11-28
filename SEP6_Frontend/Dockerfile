FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SEP6_Frontend.csproj", "SEP6_Frontend/"]
RUN dotnet restore "SEP6_Frontend/SEP6_Frontend.csproj"
COPY . SEP6_Frontend
WORKDIR "/src/SEP6_Frontend"
RUN dotnet build "SEP6_Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SEP6_Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SEP6_Frontend.dll"]
