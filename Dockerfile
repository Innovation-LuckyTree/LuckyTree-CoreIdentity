# Use the official Microsoft .NET Core SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and individual project files 
COPY *.sln .
COPY CoreIdentity.API/ CoreIdentity.API/
COPY CoreIdentity.Application/ CoreIdentity.Application/
COPY CoreIdentity.Common/ CoreIdentity.Common/
COPY CoreIdentity.Domain/ CoreIdentity.Domain/
COPY CoreIdentity.Infrastructure/ CoreIdentity.Infrastructure/
COPY CoreIdentity.Persistence/ CoreIdentity.Persistence/
# Restore NuGet packages for the entire solution
RUN dotnet restore


# Copy everything else and build
COPY . ./
RUN dotnet publish CoreIdentity.API -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

Expose 8080

ENTRYPOINT ["dotnet", "CoreIdentity.API.dll"]

