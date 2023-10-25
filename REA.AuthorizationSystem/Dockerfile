FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["REA.AuthorizationSystem/REA.AuthorizationSystem.csproj", "REA.AuthorizationSystem/"]
COPY ["REA.AuthorizationSystem.BLL/REA.AuthorizationSystem.BLL.csproj", "REA.AuthorizationSystem.BLL/"]
COPY ["REA.AuthorizationSystem.DAL/REA.AuthorizationSystem.DAL.csproj", "REA.AuthorizationSystem.DAL/"]
COPY ["ApiGateWay/ApiGateWay.csproj", "ApiGateWay/"]
RUN dotnet restore "REA.AuthorizationSystem/REA.AuthorizationSystem.csproj"
COPY . .
WORKDIR "/src/REA.AuthorizationSystem"
RUN dotnet build "REA.AuthorizationSystem.csproj" -c Release -o /app/build

#WORKDIR "/src"
#RUN dotnet dotnet-ef migrations add AuthorizationServiceMigration -p REA.AuthorizationSystem.DAL -s REA.AuthorizationSystem -c AgencyIdentityContext
#RUN dotnet ef database update -p REA.AuthorizationSystem.DAL -s REA.AuthorizationSystem -c AgencyIdentityContext

FROM build AS publish
RUN dotnet publish "REA.AuthorizationSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "REA.AuthorizationSystem.dll"]
