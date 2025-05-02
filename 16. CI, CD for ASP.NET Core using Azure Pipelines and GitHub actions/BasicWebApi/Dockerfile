#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#stage 1: Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BasicWebApi.csproj", "."]
RUN dotnet restore "./BasicWebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./BasicWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

#stage 1: Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BasicWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#stage 1: Run stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasicWebApi.dll"]