FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /CoreApp

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /CoreApp
COPY --from=build-env /CoreApp/out .
ENTRYPOINT ["dotnet", "CoreApp.dll"]