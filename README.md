# 💰 Finance Control System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14-336791?logo=postgresql)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> Sistema completo de controle financeiro pessoal desenvolvido com Clean Architecture, Domain-Driven Design (DDD) e CQRS pattern.

## 🎯 Sobre o Projeto

Aplicação full-stack para gestão financeira pessoal focada em demonstrar boas práticas de desenvolvimento de software, incluindo:

- ✅ **Clean Architecture** - Separação clara de responsabilidades
- ✅ **Domain-Driven Design (DDD)** - Modelagem rica de domínio
- ✅ **CQRS** - Separação de comandos e consultas com MediatR
- ✅ **Event-Driven** - Domain Events pattern
- ✅ **TDD** - Desenvolvimento orientado a testes

## 🚀 Funcionalidades

- 💳 Gestão de múltiplas contas (corrente, poupança, carteira, investimentos)
- 💸 Controle de transações (receitas, despesas, transferências)
- 📊 Categorização de gastos
- 📈 Relatórios e dashboards
- 🎯 Metas financeiras
- 🔔 Notificações e alertas

## 🏗️ Arquitetura

### Camadas
```
src/
├── FinanceControl.Domain/          # Entidades, Value Objects, Domain Events
├── FinanceControl.Application/     # Use Cases (CQRS), DTOs, Validators
├── FinanceControl.Infrastructure/  # EF Core, Repositories, External Services
└── FinanceControl.API/            # REST API, Controllers, Middlewares
```

### Tecnologias

**Back-end:**
- .NET 8
- Entity Framework Core
- PostgreSQL
- Redis (Cache)
- RabbitMQ (Mensageria)
- Hangfire (Background Jobs)
- MediatR (CQRS)
- FluentValidation
- Serilog

**Front-end** (Em desenvolvimento):
- Nuxt 3
- Vue 3
- Tailwind CSS
- Chart.js

## 🐳 Docker
```bash
# Iniciar serviços
docker-compose up -d

# Parar serviços
docker-compose down
```

**Serviços disponíveis:**
- PostgreSQL: `localhost:5432`
- pgAdmin: `http://localhost:5050`
- Redis: `localhost:6379`
- RabbitMQ: `localhost:5672` | Management UI: `http://localhost:15672`

## 🔧 Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Passos

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/finance-control-system.git
cd finance-control-system
```

2. **Inicie os serviços Docker**
```bash
docker-compose up -d
```

3. **Configure as variáveis de ambiente**
```bash
cp .env.example .env
# Edite o arquivo .env conforme necessário
```

4. **Execute as migrations**
```bash
cd src/FinanceControl.API
dotnet ef database update
```

5. **Execute a API**
```bash
dotnet run
```

6. **Acesse o Swagger**
```
https://localhost:5001/swagger
```

## 🧪 Testes
```bash
# Executar todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true
```

## 📁 Estrutura do Projeto
```
FinanceControl/
├── src/
│   ├── FinanceControl.Domain/
│   │   ├── Entities/           # Account, Transaction, Category
│   │   ├── ValueObjects/       # Money
│   │   ├── Events/             # Domain Events
│   │   ├── Enums/              # AccountType, TransactionType
│   │   └── Exceptions/         # DomainException
│   ├── FinanceControl.Application/
│   │   ├── UseCases/           # CQRS Commands/Queries
│   │   ├── DTOs/               # Data Transfer Objects
│   │   └── Validators/         # FluentValidation
│   ├── FinanceControl.Infrastructure/
│   │   ├── Persistence/        # DbContext, Repositories
│   │   └── Services/           # External integrations
│   └── FinanceControl.API/
│       └── Controllers/        # REST endpoints
├── tests/
│   ├── FinanceControl.Domain.Tests/
│   ├── FinanceControl.Application.Tests/
│   └── FinanceControl.Integration.Tests/
└── docker-compose.yml
```

## 🎓 Conceitos Aplicados

### Domain-Driven Design
- **Aggregates**: `Account` como raiz de agregado
- **Value Objects**: `Money` com validações de moeda
- **Domain Events**: `AccountCreated`, `TransactionCreated`, `TransferCompleted`
- **Entities**: `Transaction`, `Category`

### Clean Architecture
- Dependências sempre apontam para o centro (Domain)
- Domain não conhece Infrastructure ou Application
- Inversão de dependência com interfaces

### CQRS
- Commands para modificar estado
- Queries para leitura
- MediatR como mediador

## 📊 Status do Projeto

🚧 **Em desenvolvimento ativo**

- [x] Estrutura do projeto
- [x] Domain layer (Entities, Value Objects, Events)
- [x] Docker services
- [ ] Application layer (Use Cases, Validators)
- [ ] Infrastructure layer (EF Core, Repositories)
- [ ] API REST
- [ ] Front-end (Nuxt 3)
- [ ] Testes completos
- [ ] CI/CD

## 🤝 Contribuindo

Contribuições são bem-vindas! Veja [CONTRIBUTING.md](CONTRIBUTING.md) para mais detalhes.

## 📝 Licença

Este projeto está sob a licença MIT. Veja [LICENSE](LICENSE) para mais informações.

## 👨‍💻 Autor

**Seu Nome**
- GitHub: [@RichardFurlan](https://github.com/RichardFurlan)
- LinkedIn: [RichardFurlan](https://www.linkedin.com/in/richard-furlan-0715107b/)

---

⭐ Se este projeto foi útil, considere dar uma estrela!
```

