# # Stage 1: Build the .NET application
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["KwFeeds.csproj", "./"]
# RUN dotnet restore "KwFeeds.csproj"
# COPY . .
# RUN dotnet build "KwFeeds.csproj" -c Release -o /app/build

# # Stage 2: Publish the .NET application
# FROM build AS publish
# RUN dotnet publish "KwFeeds.csproj" -c Release -o /app/publish /p:UseAppHost=false

# # Stage 3: Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# # Copy the license file into the container
# COPY license.txt /app/license.txt

# # Add Microsoft package repository for .NET and install necessary tools
# RUN apt-get update && \
#     apt-get install -y wget gnupg && \
#     wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb && \
#     dpkg -i packages-microsoft-prod.deb && \
#     apt-get update && \
#     apt-get install -y dotnet-sdk-6.0

# # Install the specific version of Kentico Database Manager Tool (matching project version)
# RUN dotnet tool install --global Kentico.Xperience.DbManager --version 29.1.3

# # Ensure the .dotnet/tools directory is in the PATH for the root user
# ENV PATH="/root/.dotnet/tools:$PATH"

# # Set environment variables for sensitive information
# ENV DB_PASSWORD="EmmyConcept_55555"
# ENV ADMIN_PASSWORD="EmmyConcept_55555"

# # Run the database manager command
# RUN /root/.dotnet/tools/kentico-xperience-dbmanager -- \
#     -d KwFeeds_2024-06-26T08-42Z \
#     -u CloudSA71d9f3dc \
#     -s "tcp:kwfeeds.database.windows.net,1433" \
#     -p "$DB_PASSWORD" \
#     -a "$ADMIN_PASSWORD" \
#     --license-file "/app/license.txt" \
#     --use-existing-database

# # Create a non-root user and set permissions
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

# # Specify the entry point for your application
# ENTRYPOINT ["dotnet", "KwFeeds.dll"]


# dotnet kentico-xperience-dbmanager -- -s "tcp:EC2AMAZ-HCLCP8V,1433" -d "KwFeeds" -a "KwFeeds" --hash-string-salt "kwfeeds" --license-file "license.txt" --use-existing-database














# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KwFeeds.csproj", "./"]
RUN dotnet restore "KwFeeds.csproj"
COPY . .
RUN dotnet build "KwFeeds.csproj" -c Release -o /app/build

# Stage 2: Publish the .NET application
FROM build AS publish
RUN dotnet publish "KwFeeds.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the license file into the container
COPY license.txt /app/license.txt

# Set environment variables for sensitive information and configuration
ENV DB_PASSWORD="QKfDN3Jy~5P}dh}f" \
    ADMIN_PASSWORD="EmmyConcept_55555" \
    ASPNETCORE_ENVIRONMENT=Production \
    KENTICO_LICENSE_PATH="/app/license.txt"

# Install necessary tools and set up the environment
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

# Run the database manager command using the installed tool
RUN /root/.dotnet/tools/kentico-xperience-dbmanager -- \
    -d KwFeeds \
    -u EC2AMAZ-HCLCP8V\external_admin \
    -s "tcp:34.249.232.53" \
    -p "$DB_PASSWORD" \
    -a "$ADMIN_PASSWORD" \
    --license-file "/app/license.txt" \
    --use-existing-database

# Create a non-root user and set permissions
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Specify the entry point for your application
ENTRYPOINT ["dotnet", "KwFeeds.dll"]