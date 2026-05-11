# Cafeteria Gato do Luar – API REST

> Desafio Sprint 03 – Desenvolvimento de APIs e Serviços Web  
> Estudante: Amanda

---

## Arquitetura

O projeto segue a arquitetura em camadas (Layered Architecture):

```
Interface Web (Bootstrap + JS)
         ↓ HTTP (JSON)
    Controllers          ← recebe requisições, valida entrada, retorna respostas
         ↓
     Services            ← regras de negócio, validações de domínio
         ↓
   Repositories          ← acesso ao banco via Entity Framework Core
         ↓
      MySQL DB            ← banco cafeteria_db
```

### Estrutura de pastas

```
CafeteriaAPI/
├── Controllers/          # Endpoints da API
├── Models/               # Entidades do banco de dados
├── DTOs/                 # Objetos de transferência (entrada/saída da API)
├── Services/             # Regras de negócio
│   └── Interfaces/
├── Repositories/         # Acesso ao banco de dados
│   └── Interfaces/
├── Data/                 # AppDbContext (EF Core)
├── Helpers/              # JwtHelper (geração de tokens)
├── Middleware/           # Tratamento global de erros
├── wwwroot/              # Interface web (HTML + Bootstrap)
└── appsettings.json      # Configurações (conexão, JWT)
```

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 8 |
| ORM | Entity Framework Core 8 |
| Banco de dados | MySQL (Pomelo driver) |
| Autenticação | JWT Bearer |
| Documentação | Swagger / OpenAPI |
| Frontend | HTML5 + Bootstrap 5 + Vanilla JS |

---

## Como executar

### Pré-requisitos
- .NET 8 SDK
- MySQL rodando localmente

### Passos

```bash
# 1. Clonar o repositório
git clone https://github.com/seu-usuario/CafeteriaAPI

# 2. Configurar a connection string
# Edite appsettings.json e coloque sua senha do MySQL

# 3. Restaurar pacotes
dotnet restore

# 4. Aplicar migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# 5. Rodar a API
dotnet run

# 6. Acessar o Swagger
# http://localhost:5000/swagger
```

---

## Endpoints principais

### Auth
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | /api/auth/login | Login → retorna token JWT | ❌ |
| POST | /api/auth/registrar | Registra funcionário | ✅ Gerente |

### Produtos
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/produtos | Lista produtos | ❌ |
| GET | /api/produtos/{id} | Busca por ID | ❌ |
| POST | /api/produtos | Cria produto | ✅ Gerente |
| PUT | /api/produtos/{id} | Atualiza produto | ✅ Gerente |
| DELETE | /api/produtos/{id} | Desativa produto | ✅ Gerente |

### Pedidos
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/pedidos | Lista pedidos | ✅ |
| GET | /api/pedidos/{id} | Busca pedido com itens | ✅ |
| POST | /api/pedidos | Abre pedido | ✅ |
| PUT | /api/pedidos/{id}/status | Atualiza status | ✅ |
| DELETE | /api/pedidos/{id} | Cancela pedido | ✅ |

---

## Segurança

- Autenticação via **JWT Bearer Token**
- Expiração configurável (padrão: 8h)
- Autorização por **role (cargo)**: gerentes têm acesso a rotas administrativas
- Senhas armazenadas com **BCrypt hash**

---

## Padrão de resposta

Todos os endpoints retornam o mesmo formato:

```json
{
  "sucesso": true,
  "mensagem": "Produto criado com sucesso.",
  "dados": { ... }
}
```
