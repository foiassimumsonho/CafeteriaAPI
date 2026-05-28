# Cafeteria Gato do Luar – API REST

> Projeto desenvolvido ao longo dos Sprints 02, 03 e 04 do curso de Desenvolvimento Backend.  
> Estudante: Amanda

---

## 🌐 Acesso em Produção

**Interface Web:** https://cafeteriaapi-m4xx.onrender.com  
**Swagger (documentação da API):** https://cafeteriaapi-m4xx.onrender.com/swagger  
**Banco de dados:** FreeSQLDatabase (MySQL 5.5 — nuvem)

> ⚠️ O plano gratuito do Render hiberna após inatividade. Na primeira requisição pode demorar até 50 segundos para acordar.

---

## 📋 Descrição do Projeto

Sistema web completo de gestão para a **Cafeteria Gato do Luar**, desenvolvido com ASP.NET Core 8 e MySQL. Permite o gerenciamento de produtos, clientes, funcionários, pedidos e pagamentos, com autenticação JWT, interface web responsiva e relatório financeiro mensal.

---

## 🎯 Objetivo do Sistema

Digitalizar e centralizar a operação da cafeteria, permitindo que funcionários registrem pedidos, controlem o estoque, gerenciem clientes e acompanhem o faturamento em tempo real.

---

## 🛠️ Tecnologias Utilizadas

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 8 |
| ORM | Entity Framework Core 8 |
| Banco de dados | MySQL (Pomelo driver) |
| Autenticação | JWT Bearer |
| Hash de senhas | BCrypt.Net-Next |
| Documentação | Swagger / OpenAPI |
| Frontend | HTML5 + Bootstrap 5 + Vanilla JS |
| Deploy API | Render (Docker) |
| Banco produção | FreeSQLDatabase.com |
| Versionamento | Git + GitHub |

---

## 🏗️ Arquitetura

```
Interface Web (Bootstrap + JS)
         ↓ HTTP (JSON)
    Controllers          ← recebe requisições, retorna respostas JSON
         ↓
     Services            ← regras de negócio e validações
         ↓
   Repositories          ← acesso ao banco via Entity Framework Core
         ↓
      MySQL DB            ← banco remoto (FreeSQLDatabase)
```

### Estrutura de Pastas

```
CafeteriaAPI/
├── Controllers/          # Endpoints da API
│   ├── AuthController.cs
│   ├── ClientesController.cs
│   ├── DashboardController.cs
│   ├── FuncionariosController.cs
│   ├── PagamentosController.cs
│   ├── PedidosController.cs
│   ├── ProdutosController.cs
│   └── RelatorioController.cs
├── Models/               # Entidades do banco de dados
├── DTOs/                 # Objetos de transferência
├── Services/             # Regras de negócio
│   └── Interfaces/
├── Repositories/         # Acesso ao banco de dados
│   └── Interfaces/
├── Data/                 # AppDbContext (EF Core)
├── Helpers/              # JwtHelper
├── Middleware/           # Tratamento global de erros
├── Migrations/           # Migrations do EF Core
├── wwwroot/              # Interface web (HTML + Bootstrap)
├── Dockerfile            # Configuração para deploy no Render
└── appsettings.json      # Configurações
```

---

## ⚙️ Como Executar Localmente

### Pré-requisitos
- .NET 8 SDK
- MySQL rodando localmente
- Git

### Passos

```bash
# 1. Clonar o repositório
git clone https://github.com/foiassimumsonho/CafeteriaAPI

# 2. Entrar na pasta
cd CafeteriaAPI

# 3. Configurar a connection string
# Edite appsettings.json com sua senha do MySQL local

# 4. Restaurar pacotes
dotnet restore

# 5. Aplicar migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# 6. Rodar a API
dotnet run

# 7. Acessar
# Interface: http://localhost:5000
# Swagger:   http://localhost:5000/swagger
```

---

## 🔐 Autenticação e Segurança

- Autenticação via **JWT Bearer Token** com expiração de 8 horas
- Senhas armazenadas com **BCrypt hash**
- Autorização por cargo — gerentes têm acesso a rotas administrativas
- **Conformidade com a LGPD** — dados pessoais (nome, e-mail, CPF) coletados exclusivamente para gestão interna
- CPF e e-mail **mascarados** na interface para proteção de dados sensíveis

**Credenciais de demonstração:**
```
Email: amanda@cafeteria.com
Senha: cafeteria123
```

---

## 📡 Endpoints da API

### Auth
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | /api/Auth/login | Login → retorna JWT | ❌ |
| POST | /api/Auth/registrar | Registra funcionário | ✅ Gerente |

### Produtos
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Produtos | Lista produtos | ❌ |
| GET | /api/Produtos/{id} | Busca por ID | ❌ |
| POST | /api/Produtos | Cria produto | ✅ Gerente |
| PUT | /api/Produtos/{id} | Atualiza produto | ✅ Gerente |
| DELETE | /api/Produtos/{id} | Desativa produto | ✅ Gerente |

### Clientes
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Clientes | Lista clientes | ✅ |
| GET | /api/Clientes/{id} | Busca por ID | ✅ |
| POST | /api/Clientes | Cadastra cliente | ✅ |
| PUT | /api/Clientes/{id} | Atualiza cliente | ✅ |
| DELETE | /api/Clientes/{id} | Desativa cliente | ✅ |

### Funcionários
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Funcionarios | Lista funcionários | ✅ |
| GET | /api/Funcionarios/{id} | Busca por ID | ✅ |
| POST | /api/Funcionarios | Cadastra funcionário | ✅ Gerente |
| PUT | /api/Funcionarios/{id} | Atualiza funcionário | ✅ Gerente |
| DELETE | /api/Funcionarios/{id} | Desativa funcionário | ✅ Gerente |

### Pedidos
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Pedidos | Lista pedidos | ✅ |
| GET | /api/Pedidos/{id} | Busca pedido com itens | ✅ |
| POST | /api/Pedidos | Abre pedido | ✅ |
| PUT | /api/Pedidos/{id}/status | Atualiza status | ✅ |
| DELETE | /api/Pedidos/{id} | Cancela pedido | ✅ |

### Pagamentos
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Pagamentos | Lista pagamentos | ✅ |
| POST | /api/Pagamentos | Registra pagamento | ✅ |

### Dashboard
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Dashboard | Resumo do dia | ✅ |

### Relatório
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | /api/Relatorio?ano=2026&mes=5 | Relatório financeiro mensal | ✅ |

---

## 📐 Regras de Negócio

- Apenas **Gerentes** podem criar/editar/desativar produtos e funcionários
- Ao criar um pedido, o **estoque é reduzido automaticamente**
- Ao cancelar um pedido, o **estoque é devolvido automaticamente**
- Um pedido cancelado **não pode ser reaberto**
- Cada pedido só pode ter **um pagamento registrado**
- Senhas armazenadas com **BCrypt hash** — nunca em texto puro
- Tokens JWT expiram em **8 horas**

---

## 🖥️ Funcionalidades da Interface Web

- **Dashboard** — resumo do dia (pedidos, faturamento, status por categoria)
- **Produtos** — listagem, cadastro, edição e desativação
- **Clientes** — listagem com dados mascarados (LGPD), cadastro, edição e desativação
- **Pedidos** — abertura com múltiplos itens, atualização de status e cancelamento
- **Pagamentos** — registro com forma (Dinheiro, Crédito, Débito, Pix)
- **Funcionários** — listagem, cadastro e edição (apenas Gerentes)
- **Relatório Financeiro** — faturamento mensal, produtos mais vendidos, pedidos entregues e cancelados
- **Interface responsiva** — funciona em desktop, tablet e celular
- **Banner LGPD** — aviso de coleta de dados na primeira visita

---

## 🔗 Links

- **Repositório:** https://github.com/foiassimumsonho/CafeteriaAPI
- **Sistema online:** https://cafeteriaapi-m4xx.onrender.com
- **Swagger:** https://cafeteriaapi-m4xx.onrender.com/swagger
