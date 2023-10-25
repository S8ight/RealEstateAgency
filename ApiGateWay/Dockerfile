FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateWay/ApiGateWay.csproj", "ApiGateWay/"]
RUN dotnet restore "ApiGateWay/ApiGateWay.csproj"
COPY . .
WORKDIR "/src/ApiGateWay"
RUN dotnet build "ApiGateWay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateWay.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateWay.dll"]
