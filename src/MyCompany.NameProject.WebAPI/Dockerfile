#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY ["MyCompany.NameProject.sln" "MyCompany.NameProject.sln"]

COPY ["MyCompany.NameProject.WebAPI/MyCompany.NameProject.WebAPI.csproj", "MyCompany.NameProject.WebAPI/"]
COPY ["MyCompany.NameProject.Application/MyCompany.NameProject.Application.csproj", "MyCompany.NameProject.Application/"]
COPY ["MyCompany.NameProject.Domain/MyCompany.NameProject.Domain.csproj", "MyCompany.NameProject.Domain/"]
COPY ["MyCompany.NameProject.Infrastructure/MyCompany.NameProject.Infrastructure.csproj", "MyCompany.NameProject.Infrastructure/"]

COPY ["docker-compose.dcproj" "docker-compose.dcproj"]

COPY ["NuGet.config" "NuGet.config"]

RUN dotnet restore "MyCompany.NameProject.sln"

COPY . .
WORKDIR "/src/MyCompany.NameProject.WebAPI"
RUN dotnet build "MyCompany.NameProject.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyCompany.NameProject.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyCompany.NameProject.WebAPI.dll"]