# ================================
# ETAPA 1: COMPILAR EL PROYECTO
# ================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["TiendaNaval.csproj", "./"]

RUN dotnet restore "TiendaNaval.csproj"

COPY . .

RUN dotnet publish "TiendaNaval.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ================================
# ETAPA 2: EJECUTAR LA APLICACIÓN
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000

ENTRYPOINT ["dotnet", "TiendaNaval.dll"]