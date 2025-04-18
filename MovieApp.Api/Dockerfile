FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MovieApp.Api/MovieApp.Api.csproj", "MovieApp.Api/"]
COPY ["MovieApp.Application/MovieApp.Application.csproj", "MovieApp.Application/"]
COPY ["MovieApp.Domain/MovieApp.Domain.csproj", "MovieApp.Domain/"]
COPY ["MovieApp.Infrastructure/MovieApp.Infrastructure.csproj", "MovieApp.Infrastructure/"]
COPY ["MovieApp.Presentation/MovieApp.Presentation.csproj", "MovieApp.Presentation/"]
RUN dotnet restore "MovieApp.Api/MovieApp.Api.csproj"
COPY . .
WORKDIR "/src/MovieApp.Api"
RUN dotnet build "MovieApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MovieApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieApp.Api.dll"]
