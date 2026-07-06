# FinControl API

Backend do FinControl, uma aplicação de controle financeiro pessoal. Construído com ASP.NET Core Web API, Entity Framework Core e PostgreSQL (Supabase).

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Supabase)
- Swagger / OpenAPI

## Estrutura do projeto

```
FinControl.API/
├── Controllers/        # Endpoints da API
├── Data/               # Contexto do banco de dados (EF Core)
├── DTOs/               # Objetos de transferência de dados
├── Models/             # Entidades do domínio
├── appsettings.json                # Configurações base (sem credentials)
└── appsettings.Development.json    # Configurações locais (não sobe pro git)
```

## Configuração

1. Clone o repositório
2. Crie o arquivo `appsettings.Development.json` na pasta `FinControl.API/` com sua connection string do Supabase:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=seu-host.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua_senha"
  }
}
```

3. Aplique as migrations para criar as tabelas no banco:

```bash
dotnet ef database update
```

4. Rode a aplicação:

```bash
dotnet run
```

5. Acesse o Swagger em `https://localhost:{porta}/swagger`

## Endpoints

### Transações (`/api/transactions`)

As transações representam movimentações financeiras — entradas (receitas) e saídas (despesas).

| Método   | Rota                      | Descrição                        |
|----------|---------------------------|----------------------------------|
| `GET`    | `/api/transactions`       | Lista todas as transações        |
| `GET`    | `/api/transactions/{id}`  | Busca uma transação pelo ID      |
| `POST`   | `/api/transactions`       | Cria uma nova transação          |
| `PUT`    | `/api/transactions/{id}`  | Atualiza uma transação existente |
| `DELETE` | `/api/transactions/{id}`  | Remove uma transação             |

#### Exemplo de body para criar ou atualizar uma transação

```json
{
  "description": "Salário",
  "amount": 5000.00,
  "date": "2026-07-06T00:00:00Z",
  "type": 0
}
```

#### Campo `type`

| Valor | Significado |
|-------|-------------|
| `0`   | Receita (Income)  |
| `1`   | Despesa (Expense) |
