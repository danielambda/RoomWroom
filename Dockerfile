FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/Api/Api.csproj", "src/backend/Api/"]
COPY ["src/backend/Application/Application.csproj", "src/backend/Application/"]
COPY ["src/backend/Domain/Domain.csproj", "src/backend/Domain/"]
COPY ["src/backend/Contracts/Contracts.csproj", "src/backend/Contracts/"]
COPY ["src/backend/Infrastructure/Infrastructure.csproj", "src/backend/Infrastructure/"]
RUN dotnet restore "src/backend/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/backend/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
