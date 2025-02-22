FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TTLinkedinProfileTaker/TTLinkedinProfileTaker.csproj", "TTLinkedinProfileTaker/"]
RUN dotnet restore "TTLinkedinProfileTaker/TTLinkedinProfileTaker.csproj"
COPY . .
WORKDIR "/src/TTLinkedinProfileTaker"
RUN dotnet build "TTLinkedinProfileTaker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TTLinkedinProfileTaker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TTLinkedinProfileTaker.dll"]
