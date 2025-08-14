# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

RUN apt-get update && apt-get install -y libkrb5-3 libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*


# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app

# Use the runtime-only image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .

# Run the app
ENTRYPOINT ["dotnet", "PasswordLookupApp.dll"]

