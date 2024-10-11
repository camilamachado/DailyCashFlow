# DailyCashFlow

Este projeto é uma API para gerenciamento de fluxo de caixa.

## Pré-requisitos

Antes de começar, verifique se você possui os seguintes itens instalados em sua máquina:

- [Docker](https://www.docker.com/get-started)

## Como Começar

Para iniciar a aplicação, siga estes passos:

1. Abra um terminal ou console na raiz do projeto.
2. Execute o seguinte comando para construir e iniciar os contêineres do Docker:

   ```bash
   docker-compose up -d

3. Após a execução do comando, aguarde alguns instantes enquanto os contêineres são inicializados

## Serviços disponibilizados

O arquivo `docker-compose.yml` define os serviços da aplicação. A configuração padrão inclui:

- `auth-api`: A API de autenticação.
- `web-api`: A API principal.
- `sqlserver`: O banco de dados SQL Server.
- `portainer`: Interface de gerenciamento de contêineres Docker.

## Acessando documentação das APIs
- `auth-api`: http://localhost:5000/swagger/index.html
- `web-api`: http://localhost:5001/swagger/index.html
