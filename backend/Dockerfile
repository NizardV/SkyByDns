# Étape 1 : Le SDK pour compiler (Lourd mais nécessaire pour le build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# On copie tout le reste et on compile
COPY . .

# Use ARG to accept build configuration
ARG BUILD_CONFIGURATION=Release

RUN dotnet restore "API/API.csproj"
RUN dotnet build "API/API.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

# On publie l'application (création des DLLs finales)
FROM build AS publish
RUN dotnet publish "API/API.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish

# Étape 2 : L'image finale (Légère, juste le Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Set environment variable at build time (optional)
ARG BUILD_CONFIGURATION
ENV ASPNETCORE_ENVIRONMENT=${BUILD_CONFIGURATION}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]