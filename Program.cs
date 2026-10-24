using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<AppDbContext, AppDbContext>();
builder.Services.AddScoped<CategoriaService, CategoriaService>();
builder.Services.AddScoped<CategoriaRepository, CategoriaRepository>();

builder.Services.AddScoped<ProdutoService, ProdutoService>();
builder.Services.AddScoped<ProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<UnidadeEstabelecimentoService, UnidadeEstabelecimentoService>();
builder.Services.AddScoped<UnidadeEstabelecimentoRepository, UnidadeEstabelecimentoRepository>();

builder.Services.AddScoped<UsuarioService, UsuarioService>();
builder.Services.AddScoped<UsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<FidelidadeService, FidelidadeService>();
builder.Services.AddScoped<FidelidadeRepository, FidelidadeRepository>();

builder.Services.AddScoped<PontosFidelidadeService, PontosFidelidadeService>();
builder.Services.AddScoped<PontosFidelidadeRepository, PontosFidelidadeRepository>();

builder.Services.AddScoped<PedidoService, PedidoService>();
builder.Services.AddScoped<PedidoRepository, PedidoRepository>();

builder.Services.AddScoped<PagamentoService, PagamentoService>();
builder.Services.AddScoped<PagamentoRepository, PagamentoRepository>();

builder.Services.AddScoped<ItensPedidoService, ItensPedidoService>();
builder.Services.AddScoped<ItensPedidoRepository, ItensPedidoRepository>();

builder.Services.AddScoped<EstoqueUnidadeService, EstoqueUnidadeService>();
builder.Services.AddScoped<EstoqueUnidadeRepository, EstoqueUnidadeRepository>();

builder.Services.AddScoped<EstoqueMovimentacaoService, EstoqueMovimentacaoService>();
builder.Services.AddScoped<EstoqueMovimentacaoRepository, EstoqueMovimentacaoRepository>();

builder.Services.AddScoped<LogAuditoriaService, LogAuditoriaService>();
builder.Services.AddScoped<LogAuditoriaRepository, LogAuditoriaRepository>();

builder.Services.AddScoped<PromocoesCampanhasService, PromocoesCampanhasService>();
builder.Services.AddScoped<PromocoesCampanhasRepository, PromocoesCampanhasRepository>();

builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<AuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Cole apenas o token JWT, sem o prefixo Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var secret = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
var keyBytes = Convert.FromBase64String(secret);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
