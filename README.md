# Sistema de Controle de Estacionamento

Este projeto é composto por uma API RESTful desenvolvida em .NET 7 e uma interface web em Angular 2, permitindo o gerenciamento completo de um estacionamento: cadastro de veículos, controle de entradas e saídas, relatórios gerenciais e visualização em tempo real dos veículos no pátio.

## Tecnologias Utilizadas

### Backend
- **.NET 7.0**
- **Entity Framework Core** (SQLite)
- **FluentValidation**
- **AutoMapper**
- **Swagger/OpenAPI** (documentação automática)
- **RESTful API**

### Frontend
- **Node.js 8.17.0** (via NVM 1.1.12)
- **Angular CLI 1.0.0**
- **Angular 2.4.0**
- **PrimeNG**
- **Bootstrap 3**
- **Font Awesome**

## Como Configurar e Executar o Projeto

### Pré-requisitos
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [SQLite](https://www.sqlite.org/download.html) (necessário para o banco de dados local)
- [NVM 1.1.12](https://github.com/coreybutler/nvm-windows/releases) para gerenciar o Node.js
- Node.js 8.17.0
- Angular CLI 1.0.0 (instalado globalmente)

### 1. Backend (.NET 7)

1. Acesse a pasta do backend:
   ```bash
   cd sistema-controle-estacionamento-backend/SistemaControleEstacionamento.Api
   ```
2. Restaure os pacotes e compile:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Execute a aplicação:
   ```bash
   dotnet run
   ```
   A API estará disponível em `http://localhost:5228` (ajuste conforme o `launchSettings.json`).

### 2. Frontend (Angular 2)

1. Acesse a pasta do frontend:
   ```bash
   cd sistema-controle-estacionamento-ui
   ```
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie a aplicação:
   ```bash
   npm start
   ```
   O frontend estará disponível em `http://localhost:4200`.

> **Obs:** O frontend está configurado para consumir a API em `http://localhost:5228/api` (veja `src/environments/environment.ts`).

## Testes de API (Coleção Postman)

- Utilize os arquivos abaixo para testar todos os endpoints da API:
  - `Estacionamento_API.postman_collection.json` (coleção de requisições)
  - `Estacionamento_DEV.postman_environment.json` (variáveis de ambiente)

Importe ambos no Postman para facilitar os testes de cadastro, movimentação, relatórios e simulação de concorrência.