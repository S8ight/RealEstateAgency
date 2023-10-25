FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["REA.AdvertSystem/REA.AdvertSystem.csproj", "REA.AdvertSystem/"]
COPY ["ApiGateWay/ApiGateWay.csproj", "ApiGateWay/"]
COPY ["REA.AdvertSystem.Application/REA.AdvertSystem.Application.csproj", "REA.AdvertSystem.Application/"]
COPY ["REA.AdvertSystem.Domain/REA.AdvertSystem.Domain.csproj", "REA.AdvertSystem.Domain/"]
COPY ["DiscountService/DiscountService.csproj", "DiscountService/"]
COPY ["REA.AdvertSystem.Infrastructure/REA.AdvertSystem.Infrastructure.csproj", "REA.AdvertSystem.Infrastructure/"]
RUN dotnet restore "REA.AdvertSystem/REA.AdvertSystem.csproj"
COPY . .
WORKDIR "/src/REA.AdvertSystem"
RUN dotnet build "REA.AdvertSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "REA.AdvertSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "REA.AdvertSystem.dll"]
