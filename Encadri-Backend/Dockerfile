# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Encadri-Backend.sln .
COPY Encadri-Backend/Encadri-Backend.csproj ./Encadri-Backend/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application
COPY . .

# Build the application
WORKDIR /src/Encadri-Backend
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app
COPY --from=publish /app/publish .

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port (Railway will set PORT env var)
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "Encadri-Backend.dll"]
