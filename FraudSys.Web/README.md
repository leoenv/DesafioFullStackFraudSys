# 🏦 FraudSys — Banco KRT | Sistema de Gestão de Limite PIX

Sistema fullstack desenvolvido em **.NET 8 MVC** para gerenciar limites PIX de contas bancárias, validar transações e garantir segurança nas operações do Banco KRT.

---

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Stack e Tecnologias](#stack-e-tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação das Ferramentas](#instalação-das-ferramentas)
- [Como Rodar Localmente](#como-rodar-localmente)
- [Como Usar o Sistema](#como-usar-o-sistema)
- [Endpoints da API](#endpoints-da-api)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Rodando os Testes](#rodando-os-testes)
- [Como Subir no GitHub](#como-subir-no-github)

---

## Sobre o Projeto

O **FraudSys** é um sistema de gestão de limite PIX desenvolvido para o Banco KRT. Ele permite cadastrar contas com limites personalizados e validar se uma transação PIX pode ser realizada com base no saldo disponível.

Quando uma transação é aprovada, o valor é automaticamente debitado do limite. Quando negada, o limite permanece intacto.

---

## Funcionalidades

- ✅ Cadastrar conta com limite PIX
- ✅ Consultar dados e limite de uma conta
- ✅ Atualizar limite PIX
- ✅ Remover conta
- ✅ Validar transação PIX (aprova e debita / nega e mantém)
- ✅ API REST documentada com Swagger
- ✅ Interface web completa com validações

---

## Stack e Tecnologias

| Camada | Tecnologia |
|--------|-----------|
| Backend | .NET 8 / ASP.NET Core MVC |
| Frontend | Razor Views + Bootstrap 5 |
| Banco de dados | AWS DynamoDB (Local para desenvolvimento) |
| Documentação API | Swagger / OpenAPI |
| Testes | xUnit + Moq + FluentAssertions |
| Infraestrutura local | Docker Desktop |

---

## Arquitetura

O projeto segue os princípios de **Clean Code**, **SOLID** e uma arquitetura em camadas inspirada em **DDD (Domain-Driven Design)**:

```
FraudSys/
├── FraudSys.Domain/           # Entidades e regras de negócio puras
│   ├── Entities/
│   │   └── AccountLimit.cs    # Entidade principal com regras de limite
│   └── Interfaces/
│       └── ILimitRepository.cs
│
├── FraudSys.Application/      # Casos de uso e serviços
│   ├── DTOs/                  # Objetos de transferência de dados
│   └── Services/
│       ├── LimitService.cs
│       └── PixTransactionService.cs
│
├── FraudSys.Infrastructure/   # Acesso a dados (DynamoDB)
│   ├── DynamoDB/
│   │   └── DynamoDbConfig.cs
│   └── Repositories/
│       └── DynamoLimitRepository.cs
│
├── FraudSys.Web/              # Interface web e API REST
│   ├── Controllers/
│   │   ├── LimitController.cs
│   │   ├── PixTransactionController.cs
│   │   └── Api/
│   │       ├── LimitsApiController.cs
│   │       └── PixApiController.cs
│   └── Views/
│       ├── Limit/
│       ├── PixTransaction/
│       └── Shared/
│
└── FraudSys.Tests/            # Testes unitários
    ├── Domain/
    └── Services/
```

### Regra de negócio principal

```
Se valor da transação > limite disponível → NEGAR (limite intacto)
Se valor da transação <= limite disponível → APROVAR (limite debitado)
```

---

## Pré-requisitos

Antes de começar, você precisará ter instalado na sua máquina:

| Ferramenta | Versão | Para que serve |
|-----------|--------|---------------|
| .NET 8 SDK | 8.0 ou superior | Rodar a aplicação |
| Docker Desktop | Qualquer versão recente | Rodar o DynamoDB Local |
| AWS CLI | v2 | Criar e gerenciar tabelas locais |
| Git | Qualquer versão | Clonar e versionar o projeto |
| VS Code ou Visual Studio 2022 | — | Editar o código |

---

## Instalação das Ferramentas

### 1. .NET 8 SDK
Acesse https://dotnet.microsoft.com/download e baixe o **.NET 8 SDK**.

Verifique a instalação:
```bash
dotnet --version
# Deve mostrar: 8.x.x
```

### 2. Docker Desktop
Acesse https://www.docker.com/products/docker-desktop e instale o Docker Desktop.

Após instalar, abra o Docker Desktop e aguarde a inicialização (ícone da baleia na barra de tarefas).

Verifique a instalação:
```bash
docker --version
# Deve mostrar: Docker version xx.x.x
```

### 3. AWS CLI
Acesse https://aws.amazon.com/cli/ e instale a versão 2.

Após instalar, configure com credenciais fictícias (usadas apenas localmente):
```bash
aws configure
```

Preencha assim:
```
AWS Access Key ID: fake
AWS Secret Access Key: fake
Default region name: us-east-1
Default output format: json
```

Verifique a instalação:
```bash
aws --version
# Deve mostrar: aws-cli/2.x.x
```

### 4. Git
Acesse https://git-scm.com/downloads e instale o Git.

Verifique a instalação:
```bash
git --version
# Deve mostrar: git version x.x.x
```

---

## Como Rodar Localmente

### Passo 1 — Clone o repositório

```bash
git clone https://github.com/SEU_USUARIO/FraudSys.git
cd FraudSys
```

### Passo 2 — Suba o DynamoDB Local

Abra o Docker Desktop e aguarde inicializar. Depois rode:

```bash
docker run -d -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb
```

Verifique se está rodando:
```bash
docker ps
# Deve aparecer o container amazon/dynamodb-local
```

### Passo 3 — Crie a tabela no DynamoDB Local

```bash
aws dynamodb create-table \
  --table-name PixLimits \
  --attribute-definitions AttributeName=PK,AttributeType=S \
  --key-schema AttributeName=PK,KeyType=HASH \
  --billing-mode PAY_PER_REQUEST \
  --endpoint-url http://localhost:8000
```

Confirme que a tabela foi criada:
```bash
aws dynamodb list-tables --endpoint-url http://localhost:8000
# Deve mostrar: { "TableNames": ["PixLimits"] }
```

### Passo 4 — Rode a aplicação

```bash
cd FraudSys.Web
dotnet run
```

Você verá no terminal:
```
Now listening on: http://localhost:5083
Application started.
```

### Passo 5 — Acesse no navegador

| URL | Descrição |
|-----|-----------|
| http://localhost:5083 | Interface web principal |
| http://localhost:5083/swagger | Documentação da API |

---

## Como Usar o Sistema

### Cadastrar uma conta
1. Acesse http://localhost:5083
2. Clique em **Cadastrar Limite PIX** na navbar
3. Preencha CPF (11 dígitos), agência, conta e limite
4. Clique em **Cadastrar**

### Consultar uma conta
1. Clique em **Consultar Conta** na navbar
2. Informe agência e conta
3. Clique em **Consultar**

### Validar uma transação PIX
1. Clique em **Validar PIX** na navbar
2. Informe agência, conta e valor
3. Clique em **Validar transação**
4. O sistema exibirá se foi aprovada ou negada e o limite restante

### Editar ou remover
- Na tela de detalhes da conta, use os botões **Editar limite** ou **Remover**

---

## Endpoints da API

Acesse a documentação completa em http://localhost:5083/swagger

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/limits` | Cadastrar limite |
| `GET` | `/api/limits/{agency}/{accountNumber}` | Consultar conta |
| `PUT` | `/api/limits/{agency}/{accountNumber}` | Atualizar limite |
| `DELETE` | `/api/limits/{agency}/{accountNumber}` | Remover conta |
| `POST` | `/api/pix/validate` | Validar transação PIX |

### Exemplo — Cadastrar limite
```bash
curl -X POST http://localhost:5083/api/limits \
  -H "Content-Type: application/json" \
  -d '{
    "document": "12345678901",
    "agency": "0001",
    "accountNumber": "123456",
    "pixLimit": 5000.00
  }'
```

### Exemplo — Validar PIX
```bash
curl -X POST http://localhost:5083/api/pix/validate \
  -H "Content-Type: application/json" \
  -d '{
    "agency": "0001",
    "accountNumber": "123456",
    "amount": 300.00
  }'
```

Resposta aprovada:
```json
{
  "approved": true,
  "message": "Transação PIX aprovada com sucesso.",
  "currentLimit": 4700.00
}
```

Resposta negada:
```json
{
  "approved": false,
  "message": "Transação negada por limite PIX insuficiente.",
  "currentLimit": 5000.00
}
```

---

## Rodando os Testes

```bash
cd FraudSys
dotnet test
```

Os testes cobrem:
- Cadastro com sucesso
- Cadastro de conta já existente
- Busca de conta existente e inexistente
- Atualização e remoção de conta
- Aprovação de PIX quando valor <= limite
- Negação de PIX quando valor > limite
- Limite não consumido quando PIX negado
- Aprovação quando valor igual ao limite
- Conta não encontrada na validação

---


## Observações importantes

- O DynamoDB Local roda em memória — ao parar o container, os dados são perdidos
- Sempre suba o Docker antes de rodar a aplicação
- As credenciais AWS usadas localmente são fictícias e servem apenas para o ambiente de desenvolvimento

---

