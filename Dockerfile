
# Etapa 1: compilacao
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PetVille.API/PetVille.API.csproj", "PetVille.API/"]
RUN dotnet restore "PetVille.API/PetVille.API.csproj"

COPY . .
WORKDIR /src/PetVille.API

RUN dotnet publish "PetVille.API.csproj" -c Release -o /app/publish

# Etapa 2: execucao
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PetVille.API.dll"]
