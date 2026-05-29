FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TruckApi/TruckApi.csproj TruckApi/
RUN dotnet restore TruckApi/TruckApi.csproj

COPY TruckApi/ TruckApi/
RUN dotnet publish TruckApi/TruckApi.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

RUN useradd --no-create-home --shell /bin/false appuser

COPY --from=build /app .

RUN chown -R appuser:appuser /app
USER appuser

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TruckApi.dll"]
