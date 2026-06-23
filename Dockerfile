FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copia o arquivo de solução e o arquivo do projeto mantendo a estrutura
COPY Truck.slnx ./
COPY TruckApi/TruckApi.csproj TruckApi/

# 2. Faz o restore apontando para a solução ou projeto diretamente na raiz do WORKDIR
RUN dotnet restore TruckApi/TruckApi.csproj

# 3. Copia o resto dos arquivos do projeto
COPY TruckApi/ TruckApi/

# 4. Compila e publica os binários
RUN dotnet publish TruckApi/TruckApi.csproj -c Release -o /app --no-restore

# Estágio final de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Criação do usuário de segurança (Boa prática!)
RUN useradd --no-create-home --shell /bin/false appuser

# Copia os artefatos gerados no estágio anterior
COPY --from=build /app .

# Ajusta as permissões para o usuário não-root
RUN chown -R appuser:appuser /app
USER appuser

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TruckApi.dll"]