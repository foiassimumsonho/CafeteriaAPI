// =============================================================
// Program.cs – Ponto de entrada da aplicação
// Aqui configuramos todos os serviços e middlewares
// =============================================================
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CafeteriaAPI.Data;
using CafeteriaAPI.Middleware;
using CafeteriaAPI.Repositories;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services;
using CafeteriaAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── BANCO DE DADOS (Entity Framework + MySQL) ──────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ── INJEÇÃO DE DEPENDÊNCIA ─────────────────────────────────
// Repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

// Services
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();

// ── AUTENTICAÇÃO JWT ───────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSettings["Issuer"],
            ValidAudience            = jwtSettings["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GerenciadorPolicy", policy =>
        policy.RequireClaim("cargo", "Gerente", "gerente"));
});

// ── CORS (permite que o frontend consuma a API) ────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ── CONTROLLERS E SWAGGER ──────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Cafeteria Gato do Luar – API",
        Version     = "v1",
        Description = "API REST para gestão de pedidos, produtos e clientes da cafeteria"
    });

    // Configurar Swagger para usar o token JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Description = "Informe o token JWT: Bearer {seu_token}",
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── BUILD ──────────────────────────────────────────────────
var app = builder.Build();

// Middleware de tratamento de erros (deve ser o primeiro)
app.UseMiddleware<ErrorHandlingMiddleware>();

// Swagger disponível sempre (em produção restringir)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CafeteriaAPI v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();   // serve a interface web (wwwroot)
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Rota padrão aponta para o index.html
app.MapFallbackToFile("index.html");

app.Run();
