# Documentação técnica — FinControl API

Documentação completa do backend do FinControl: arquitetura, modelo de dados, autenticação e todos os endpoints disponíveis.

## Sumário

- [Visão geral](#visão-geral)
- [Arquitetura e estrutura de pastas](#arquitetura-e-estrutura-de-pastas)
- [Modelo de dados](#modelo-de-dados)
- [Autenticação e autorização](#autenticação-e-autorização)
- [Endpoints](#endpoints)
  - [Auth](#auth-apiauth)
  - [Transactions](#transactions-apitransactions)
  - [Categories](#categories-apicategories)
  - [Goals](#goals-apigoals)
  - [Budget Limits](#budget-limits-apibudget-limits)
- [Tratamento de erros](#tratamento-de-erros)
- [Configuração e variáveis de ambiente](#configuração-e-variáveis-de-ambiente)
- [Migrations](#migrations)

## Visão geral

FinControl API é o backend de uma aplicação de controle financeiro pessoal. Cada usuário autenticado gerencia seus próprios dados (transações, categorias, metas de economia e limites de orçamento), isolados por `UserId`.

Stack:

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core** com **Npgsql** (PostgreSQL, hospedado no Supabase)
- **ASP.NET Core Identity** (`IdentityCore<ApplicationUser>`) para gestão de usuários
- **JWT Bearer** para autenticação stateless
- **Swagger / OpenAPI** para documentação interativa

## Arquitetura e estrutura de pastas

```
FinControl.API/
├── Controllers/       # Endpoints HTTP (um controller por recurso)
│   ├── AuthController.cs
│   ├── TransactionsController.cs
│   ├── CategoriesController.cs
│   ├── GoalsController.cs
│   └── BudgetLimitsController.cs
├── Data/
│   └── AppDbContext.cs       # DbContext (herda de IdentityDbContext)
├── DTOs/                      # Records de entrada/saída, um arquivo por recurso
├── Extensions/
│   └── ControllerBaseExtensions.cs   # GetUserId() — extrai o Guid do usuário logado a partir do JWT
├── Models/                    # Entidades de domínio (mapeadas via EF Core)
├── Services/
│   └── JwtTokenService.cs     # Geração de tokens JWT
├── Migrations/                # Migrations do EF Core
├── Program.cs                 # Composition root: DI, autenticação, middlewares
├── appsettings.json            # Configuração base (sem segredos)
└── appsettings.Development.json  # Configuração local (fora do git)
```

Todos os controllers de recurso (exceto `AuthController`) seguem o mesmo padrão:

1. `[Authorize]` + `[ApiController]` no nível da classe.
2. Recebem `AppDbContext` via injeção de dependência no construtor primário (primary constructor).
3. Usam `this.GetUserId()` (extension method) para obter o `Guid` do usuário autenticado a partir da claim `sub` do JWT.
4. Toda query filtra por `UserId`, garantindo isolamento de dados entre usuários.
5. Validam regras de negócio simples (ex.: valores positivos) devolvendo `Problem(...)` com status 400 quando inválido.

## Modelo de dados

### `ApplicationUser` (Identity)

Estende `IdentityUser<Guid>` sem campos adicionais — usa a tabela padrão do ASP.NET Core Identity (`AspNetUsers`), com `Guid` como chave primária.

### `Transaction`

| Campo       | Tipo             | Descrição                          |
|-------------|------------------|--------------------------------------|
| `Id`        | `Guid`           | Identificador                        |
| `UserId`    | `Guid`           | Dono da transação                    |
| `Data`      | `DateOnly`       | Data da movimentação                 |
| `Tipo`      | `TransactionType`| `Receita` ou `Gasto` (enum)           |
| `Valor`     | `decimal`        | Valor (deve ser > 0)                 |
| `Categoria` | `string`         | Nome da categoria (texto livre)      |
| `Descricao` | `string`         | Descrição livre                      |

### `Category`

| Campo    | Tipo     | Descrição                              |
|----------|----------|------------------------------------------|
| `Id`     | `Guid`   | Identificador                            |
| `UserId` | `Guid`   | Dono da categoria                        |
| `Nome`   | `string` | Nome da categoria (único por usuário)    |

Índice único em `(UserId, Nome)`.

### `Goal`

| Campo    | Tipo      | Descrição                       |
|----------|-----------|-----------------------------------|
| `Id`     | `Guid`    | Identificador                     |
| `UserId` | `Guid`    | Dono da meta                      |
| `Nome`   | `string`  | Nome da meta                      |
| `Alvo`   | `decimal` | Valor alvo a ser alcançado        |
| `Salvo`  | `decimal` | Valor acumulado até o momento     |

### `BudgetLimit`

| Campo       | Tipo      | Descrição                                  |
|-------------|-----------|-----------------------------------------------|
| `Id`        | `Guid`    | Identificador                                  |
| `UserId`    | `Guid`    | Dono do limite                                 |
| `Categoria` | `string`  | Nome da categoria associada (texto livre)     |
| `Limite`    | `decimal` | Valor limite de gasto para a categoria        |

Índice único em `(UserId, Categoria)`.

Relacionamento com `Category`: quando uma categoria é removida, o `BudgetLimit` correspondente (mesmo `UserId` + `Categoria`) também é removido pelo `CategoriesController.Delete`.

## Autenticação e autorização

- Usuários se registram/autenticam via `AuthController` (`/api/auth/register`, `/api/auth/login`), usando `UserManager<ApplicationUser>` do Identity para hashing de senha e validação.
- No sucesso, `JwtTokenService.GenerateToken` emite um JWT assinado com HMAC-SHA256, contendo as claims:
  - `sub`: `Guid` do usuário
  - `email`
  - `jti`: identificador único do token
- O token expira após `Jwt:ExpiresInMinutes` minutos (configurável, padrão 120).
- Todas as demais rotas exigem o header `Authorization: Bearer {token}` e usam `[Authorize]` no controller.
- `ControllerBaseExtensions.GetUserId()` extrai o `Guid` do usuário a partir da claim `sub` (ou `ClaimTypes.NameIdentifier` como fallback) — usado em todo controller autenticado para escopar as queries.
- CORS está restrito à origem `http://localhost:3000` (frontend), configurado em `Program.cs` via a policy `Frontend`.

## Endpoints

Convenções:
- Todas as rotas, exceto `/api/auth/*`, exigem `Authorization: Bearer {token}`.
- Enums (`Tipo`) são serializados/desserializados como string em `camelCase` (`JsonStringEnumConverter`).
- Erros de validação/negócio retornam `ProblemDetails` (RFC 7807) via `Problem(...)` ou `ValidationProblem(...)`.

### Auth (`/api/auth`)

| Método | Rota                  | Auth | Descrição                         |
|--------|-----------------------|------|-------------------------------------|
| `POST` | `/api/auth/register`  | Não  | Cria um usuário e retorna o token   |
| `POST` | `/api/auth/login`     | Não  | Autentica e retorna o token         |

**Request body** (`RegisterDto` / `LoginDto`):

```json
{ "email": "usuario@exemplo.com", "password": "SenhaForte123" }
```

**Response** (`AuthResponseDto`):

```json
{
  "token": "eyJhbGciOi...",
  "expiresAt": "2026-07-08T22:00:00Z",
  "email": "usuario@exemplo.com"
}
```

Regras:
- `register`: senha mínima de 8 caracteres (`IdentityOptions.Password.RequiredLength`); erros de validação do Identity retornam 400 com detalhes por campo.
- `login`: credenciais inválidas retornam 401 (`Problem`).

### Transactions (`/api/transactions`)

| Método   | Rota                                | Descrição                                              |
|----------|--------------------------------------|-----------------------------------------------------------|
| `GET`    | `/api/transactions?month=yyyy-MM`    | Lista as transações do usuário, com filtro opcional por mês |
| `POST`   | `/api/transactions`                  | Cria uma nova transação                                    |
| `DELETE` | `/api/transactions/{id}`             | Remove uma transação                                       |

**Query param `month`**: formato `yyyy-MM` (ex.: `2026-07`). Se inválido, retorna 400.

**Create body** (`CreateTransactionDto`):

```json
{
  "tipo": "receita",
  "valor": 5000.00,
  "categoria": "Salário",
  "descricao": "Salário",
  "data": "2026-07-06"
}
```

Validação: `valor` deve ser `> 0` (400 caso contrário).

**Campo `tipo`** (`TransactionType`, serializado em camelCase):

| Valor      | Significado       |
|------------|--------------------|
| `receita`  | Receita (Income)   |
| `gasto`    | Despesa (Expense)  |

**Response** (`TransactionDto`): `{ id, data, tipo, valor, categoria, descricao }`.

`DELETE`: retorna `404` se a transação não existir ou não pertencer ao usuário; `204` em sucesso.

### Categories (`/api/categories`)

| Método   | Rota                     | Descrição                                     |
|----------|--------------------------|--------------------------------------------------|
| `GET`    | `/api/categories`        | Lista as categorias do usuário                    |
| `POST`   | `/api/categories`        | Cria uma nova categoria                            |
| `DELETE` | `/api/categories/{id}`   | Remove uma categoria                               |

**Seed automático**: se o usuário ainda não tem nenhuma categoria, `GET` cria e persiste um conjunto padrão na primeira chamada: `Alimentação`, `Moradia`, `Transporte`, `Lazer`, `Saúde`, `Compras`, `Outros`.

**Create body** (`CreateCategoryDto`): `{ "nome": "Educação" }`

Validações:
- `nome` não pode ser vazio/whitespace (400).
- Nome duplicado para o mesmo usuário retorna `409 Conflict`.

**Response** (`CategoryDto`): `{ id, nome }`.

`DELETE`: também remove o `BudgetLimit` associado à categoria (mesmo nome), se existir. Retorna `404` se a categoria não existir/não pertencer ao usuário.

### Goals (`/api/goals`)

| Método   | Rota                       | Descrição                     |
|----------|-----------------------------|---------------------------------|
| `GET`    | `/api/goals`                | Lista as metas do usuário       |
| `POST`   | `/api/goals`                | Cria uma nova meta              |
| `DELETE` | `/api/goals/{id}`           | Remove uma meta                 |
| `POST`   | `/api/goals/{id}/deposit`   | Deposita um valor na meta       |

**Create body** (`CreateGoalDto`): `{ "nome": "Viagem", "alvo": 3000.00 }`
Validação: `alvo` deve ser `> 0`. Meta é criada com `salvo = 0`.

**Deposit body** (`DepositGoalDto`): `{ "amount": 200.00 }`
Validação: `amount` deve ser `> 0`. Incrementa `Salvo` da meta (sem limite superior — pode ultrapassar `Alvo`).

**Response** (`GoalDto`): `{ id, nome, alvo, salvo }`.

### Budget Limits (`/api/budget-limits`)

| Método   | Rota                                | Descrição                                                 |
|----------|--------------------------------------|----------------------------------------------------------|
| `GET`    | `/api/budget-limits`                 | Lista os limites por categoria: `{ "categoria": limite }` |
| `PUT`    | `/api/budget-limits/{categoria}`     | Cria ou atualiza (upsert) o limite de uma categoria       |
| `DELETE` | `/api/budget-limits/{categoria}`     | Remove o limite de uma categoria                          |

**Upsert body** (`UpsertBudgetLimitDto`): `{ "limite": 800.00 }`
Validação: `limite` deve ser `> 0`.

**Response** (`BudgetLimitDto`): `{ categoria, limite }`.

`categoria` na rota é o nome da categoria (texto livre, não o `Id`).

## Tratamento de erros

- `app.UseExceptionHandler()` + `app.UseStatusCodePages()` capturam exceções não tratadas e status codes sem corpo, produzindo respostas padronizadas.
- Erros de validação e regra de negócio usam `Problem(title:, statusCode:)` ou `ValidationProblem(ModelState)`, seguindo o formato `ProblemDetails`.
- Códigos usados: `400` (validação), `401` (credenciais inválidas), `404` (recurso não encontrado/não pertence ao usuário), `409` (conflito, ex. categoria duplicada).

## Configuração e variáveis de ambiente

`appsettings.json` (versionado, sem segredos):

```json
{
  "ConnectionStrings": { "DefaultConnection": "" },
  "Jwt": {
    "Key": "",
    "Issuer": "FinControl.API",
    "Audience": "FinControl.Front",
    "ExpiresInMinutes": "120"
  }
}
```

`appsettings.Development.json` (local, fora do git — ver `.gitignore`) deve conter:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=seu-host.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua_senha"
  },
  "Jwt": {
    "Key": "uma-chave-secreta-longa-e-aleatoria"
  }
}
```

> A `Jwt:Key` precisa ter tamanho suficiente para HMAC-SHA256 (recomendado: 32+ caracteres aleatórios). Nunca faça commit de chaves reais.

## Migrations

Migrations existentes em `FinControl.API/Migrations/`:

- `InitialCreate` — schema inicial: Identity (`AspNetUsers`, etc.), `Transactions`, `Goals`, `BudgetLimits`.
- `AddCategories` — adiciona a tabela `Categories` e o índice único `(UserId, Nome)`.

Para aplicar:

```bash
dotnet ef database update
```

Para criar uma nova migration após alterar `Models/` ou `AppDbContext`:

```bash
dotnet ef migrations add NomeDaMigration
```
