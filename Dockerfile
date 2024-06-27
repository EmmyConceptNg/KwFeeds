# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 23917

# ENV ASPNETCORE_URLS=http://+:23917

# # Creates a non-root user with an explicit UID and adds permission to access the /app folder
# # For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# ARG configuration=Release
# WORKDIR /src
# COPY ["KwFeeds.csproj", "./"]
# RUN dotnet restore "KwFeeds.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "KwFeeds.csproj" -c $configuration -o /app/build

# FROM build AS publish
# ARG configuration=Release
# RUN dotnet publish "KwFeeds.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "KwFeeds.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KwFeeds.csproj", "./"]
RUN dotnet restore "KwFeeds.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "KwFeeds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KwFeeds.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Use Debian bookworm
FROM debian:bookworm AS runtime
WORKDIR /app

# Install Snapd
RUN apt-get update && apt-get install -y snapd

# Enable classic confinement for Snap
RUN snap install core20 --classic

# Install .NET SDK 6.0 using Snap
RUN sudo snap install dotnet-sdk --channel=6.0 --classic

# Create an alias for the dotnet command
RUN sudo snap alias dotnet-sdk.dotnet dotnet

# Install Kentico Database Manager
RUN dotnet tool install -g kentico-xperience-dbmanager

# Set environment variables for sensitive information
ENV DB_PASSWORD="EmmyConcept_55555"
ENV ADMIN_PASSWORD="EmmyConcept_55555"

# Run the database manager command
RUN dotnet kentico-xperience-dbmanager -- \
    -d KwFeeds_2024-06-26T08-42Z \
    -u CloudSA71d9f3dc \
    -s "tcp:kwfeeds.database.windows.net,1433" \
    -p "$DB_PASSWORD" \
    -a "$ADMIN_PASSWORD" \
    --license-file "license.txt" \
    --recreate-existing-database

# Create a non-root user and set permissions
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Specify the entry point for your application
ENTRYPOINT ["dotnet", "KwFeeds.dll"]