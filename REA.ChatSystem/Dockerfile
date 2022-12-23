FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["REA.ChatSystem/REA.ChatSystem.csproj", "REA.ChatSystem/"]
COPY ["REA.ChatSystem.BLL/REA.ChatSystem.BLL.csproj", "REA.ChatSystem.BLL/"]
COPY ["REA.ChatSystem.DAL/REA.ChatSystem.DAL.csproj", "REA.ChatSystem.DAL/"]
RUN dotnet restore "REA.ChatSystem/REA.ChatSystem.csproj"
COPY . .
WORKDIR "/src/REA.ChatSystem"
RUN dotnet build "REA.ChatSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "REA.ChatSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "REA.ChatSystem.dll"]
