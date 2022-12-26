FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DiscountGrpcService/DiscountGrpcService.csproj", "DiscountGrpcService/"]
RUN dotnet restore "DiscountGrpcService/DiscountGrpcService.csproj"
COPY . .
WORKDIR "/src/DiscountGrpcService"
RUN dotnet build "DiscountGrpcService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountGrpcService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountGrpcService.dll"]
