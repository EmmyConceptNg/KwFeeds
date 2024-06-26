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

# Use the .NET SDK 6.0 for building the project (to match the framework version)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KwFeeds.csproj", "./"]
RUN dotnet restore "KwFeeds.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "KwFeeds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KwFeeds.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET 6.0 runtime for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the license file into the container
COPY license.txt .

# Add Microsoft package repository for .NET
RUN apt-get update && \
    apt-get install -y wget gnupg && \
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0

# Install the specific version of Kentico Database Manager Tool (matching project version)
RUN dotnet tool install --global Kentico.Xperience.DbManager --version 29.1.3

# Ensure the .dotnet/tools directory is in the PATH for the root user
ENV PATH="/root/.dotnet/tools:$PATH"

# Verify the tool installation
RUN dotnet tool list -g

# Set environment variables for sensitive information
ENV DB_PASSWORD="EmmyConcept_55555"
ENV ADMIN_PASSWORD="EmmyConcept_55555"

# Run the database manager command
RUN /root/.dotnet/tools/kentico-xperience-dbmanager -- \
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