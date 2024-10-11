# Usar a imagem base do SDK .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar os arquivos de projeto necessários
COPY ../DailyCashFlow.Auth/*.csproj ./DailyCashFlow.Auth/
COPY ../DailyCashFlow.Application/*.csproj ./DailyCashFlow.Application/
COPY ../DailyCashFlow.Domain/*.csproj ./DailyCashFlow.Domain/
COPY ../DailyCashFlow.Infra.Data/*.csproj ./DailyCashFlow.Infra.Data/

# Restaurar dependências
RUN dotnet restore ./DailyCashFlow.Auth/DailyCashFlow.Auth.csproj

# Copiar todo o código fonte
COPY .. .

# Publicar a aplicação
RUN dotnet publish ./DailyCashFlow.Auth/DailyCashFlow.Auth.csproj -c Release -o out

# Estágio de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "DailyCashFlow.Auth.dll"]