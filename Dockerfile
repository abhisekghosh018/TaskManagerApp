# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything in the solution
COPY . .

# 👇 Restore using the relative path to .csproj
RUN dotnet restore "DevTaskTracker.API/DevTaskTracker.API.csproj"

# 👇 Publish the API
RUN dotnet publish "DevTaskTracker.API/DevTaskTracker.API.csproj" -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "DevTaskTracker.API.dll"]
