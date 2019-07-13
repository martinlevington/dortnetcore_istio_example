FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY docker_helloWorld/docker_helloWorld.csproj docker_helloWorld/
RUN dotnet restore docker_helloWorld/docker_helloWorld.csproj
COPY . .
WORKDIR /src/docker_helloWorld
RUN dotnet build docker_helloWorld.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish docker_helloWorld.csproj -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "docker_helloWorld.dll"]
