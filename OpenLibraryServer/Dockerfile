FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY OpenLibraryServer.Models/*.csproj ./OpenLibraryServer.Models/
COPY OpenLibraryServer.DataAccess/*.csproj ./OpenLibraryServer.DataAccess/
COPY OpenLibraryServer.Service/*.csproj ./OpenLibraryServer.Service/
COPY OpenLibraryServer.Web/*.csproj ./OpenLibraryServer.Web/
RUN dotnet restore --verbosity detailed
COPY . .
WORKDIR /src/OpenLibraryServer.Web
RUN dotnet publish -c release -o /app --no-restore --no-cache

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "OpenLibraryServer.Web.dll"]