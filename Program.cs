using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
