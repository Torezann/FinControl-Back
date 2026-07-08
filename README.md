# FinControl API

Backend do FinControl, uma aplicação de controle financeiro pessoal. Construído com ASP.NET Core Web API, Entity Framework Core e PostgreSQL (Supabase).

Permite que cada usuário gerencie, de forma isolada e autenticada: transações (receitas/despesas), categorias, metas de economia e limites de orçamento por categoria.

📖 **Documentação completa**: [docs/DOCUMENTATION.md](docs/DOCUMENTATION.md) — arquitetura, modelo de dados, autenticação e detalhamento de todos os endpoints.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + Npgsql
- ASP.NET Core Identity (usuários e senhas)
- Autenticação via JWT Bearer
- PostgreSQL (Supabase)
- Swagger / OpenAPI

## Funcionalidades

- **Autenticação**: registro e login de usuários com emissão de token JWT.
- **Transações**: CRUD de receitas e despesas, com filtro por mês.
- **Categorias**: CRUD de categorias por usuário, com seed automático de categorias padrão.
- **Metas**: criação de metas de economia e depósitos incrementais.
- **Limites de orçamento**: definição de um limite de gasto por categoria.

Todos os dados são isolados por usuário — cada request autenticado só acessa seus próprios registros.

## Estrutura do projeto

```
FinControl.API/
├── Controllers/        # Endpoints da API (Auth, Transactions, Categories, Goals, BudgetLimits)
├── Data/               # Contexto do banco de dados (EF Core + Identity)
├── DTOs/               # Objetos de transferência de dados
├── Extensions/         # Extension methods (ex.: obter o usuário autenticado)
├── Models/             # Entidades do domínio
├── Services/           # Serviços de aplicação (ex.: geração de JWT)
├── Migrations/         # Migrations do EF Core
├── appsettings.json                # Configurações base (sem credentials)
└── appsettings.Development.json    # Configurações locais (não sobe pro git)
```

## Configuração

1. Clone o repositório
2. Crie o arquivo `appsettings.Development.json` na pasta `FinControl.API/` com sua connection string do Supabase e uma chave JWT:

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
  },
  "Jwt": {
    "Key": "uma-chave-secreta-longa-e-aleatoria"
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

## Endpoints (resumo)

Todas as rotas abaixo, exceto `/api/auth/*`, exigem um header `Authorization: Bearer {token}` obtido em `/api/auth/login` ou `/api/auth/register`. Para detalhes de request/response de cada rota, veja [docs/DOCUMENTATION.md](docs/DOCUMENTATION.md#endpoints).

### Autenticação (`/api/auth`)

| Método | Rota                  | Descrição                          |
|--------|-----------------------|-------------------------------------|
| `POST` | `/api/auth/register`  | Cria um usuário e retorna o token  |
| `POST` | `/api/auth/login`     | Autentica e retorna o token         |

### Transações (`/api/transactions`)

| Método   | Rota                               | Descrição                                                  |
|----------|--------------------------------------|--------------------------------------------------------------|
| `GET`    | `/api/transactions?month=yyyy-MM`    | Lista as transações do usuário (filtro opcional por mês)   |
| `POST`   | `/api/transactions`                  | Cria uma nova transação                                      |
| `DELETE` | `/api/transactions/{id}`             | Remove uma transação                                         |

### Categorias (`/api/categories`)

| Método   | Rota                     | Descrição                                              |
|----------|--------------------------|------------------------------------------------------------|
| `GET`    | `/api/categories`        | Lista as categorias do usuário (cria as padrão se vazio)  |
| `POST`   | `/api/categories`        | Cria uma nova categoria                                    |
| `DELETE` | `/api/categories/{id}`   | Remove uma categoria (e seu limite, se houver)             |

### Metas (`/api/goals`)

| Método   | Rota                       | Descrição                     |
|----------|-----------------------------|---------------------------------|
| `GET`    | `/api/goals`                | Lista as metas do usuário       |
| `POST`   | `/api/goals`                | Cria uma nova meta              |
| `DELETE` | `/api/goals/{id}`           | Remove uma meta                 |
| `POST`   | `/api/goals/{id}/deposit`   | Deposita um valor na meta       |

### Limites de orçamento (`/api/budget-limits`)

| Método   | Rota                              | Descrição                                                  |
|----------|-------------------------------------|----------------------------------------------------------|
| `GET`    | `/api/budget-limits`                | Lista os limites por categoria (`{ categoria: limite }`) |
| `PUT`    | `/api/budget-limits/{categoria}`    | Cria ou atualiza o limite de uma categoria                |
| `DELETE` | `/api/budget-limits/{categoria}`    | Remove o limite de uma categoria                          |
