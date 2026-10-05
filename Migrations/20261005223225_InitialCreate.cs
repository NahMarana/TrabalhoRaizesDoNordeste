using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoRaizesDoNordeste.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeCategoria = table.Column<string>(type: "TEXT", nullable: false),
                    CategoriaAtiva = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesEstabelecimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeEstabelecimento = table.Column<string>(type: "TEXT", nullable: false),
                    CNPJ = table.Column<string>(type: "TEXT", nullable: false),
                    UnidadeAtiva = table.Column<bool>(type: "INTEGER", nullable: false),
                    TipoUnidade = table.Column<int>(type: "INTEGER", nullable: false),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    CEP = table.Column<string>(type: "TEXT", nullable: false),
                    TelefoneUnidade = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesEstabelecimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeProduto = table.Column<string>(type: "TEXT", nullable: false),
                    DescricaoProduto = table.Column<string>(type: "TEXT", nullable: true),
                    PrecoUnitario = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProdutoSazonal = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataInicioSazonal = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DataFimSazonal = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ProdutoAtivo = table.Column<bool>(type: "INTEGER", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
                    Perfil = table.Column<int>(type: "INTEGER", nullable: false),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    CEP = table.Column<string>(type: "TEXT", nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    ConsentimentoLGPD = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParticipaFidelidade = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsuarioAtivo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_UnidadesEstabelecimento_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "UnidadesEstabelecimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstoquesUnidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QtdEstoqueDisponivel = table.Column<int>(type: "INTEGER", nullable: false),
                    VendaDisponivel = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoquesUnidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoquesUnidade_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoquesUnidade_UnidadesEstabelecimento_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "UnidadesEstabelecimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromocoesCampanha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EstabelecimentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    ValorPromocional = table.Column<decimal>(type: "TEXT", nullable: false),
                    StatusPromoCampanha = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicio = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocoesCampanha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromocoesCampanha_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromocoesCampanha_UnidadesEstabelecimento_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "UnidadesEstabelecimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fidelidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    QtdPontos = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fidelidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fidelidades_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Acao = table.Column<string>(type: "TEXT", nullable: false),
                    EntidadeAfetada = table.Column<string>(type: "TEXT", nullable: false),
                    EntidadeAfetadaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DadosNovos = table.Column<string>(type: "TEXT", nullable: false),
                    DadosAnteriores = table.Column<string>(type: "TEXT", nullable: false),
                    DataOrigem = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsAuditoria_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CanalPedido = table.Column<int>(type: "INTEGER", nullable: false),
                    ModoReceber = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusPedido = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorTotalPedido = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataHoraPedido = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_UnidadesEstabelecimento_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "UnidadesEstabelecimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstoquesMovimentacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EstoqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioUsadoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QtdEmEstoque = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoMovimento = table.Column<int>(type: "INTEGER", nullable: false),
                    MotivoMovimentacao = table.Column<string>(type: "TEXT", nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoquesMovimentacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoquesMovimentacao_EstoquesUnidade_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "EstoquesUnidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoquesMovimentacao_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoquesMovimentacao_Usuarios_UsuarioUsadoId",
                        column: x => x.UsuarioUsadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QtdItens = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrecoTotal = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoPagamento = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusPagamento = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorPagamento = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConfirmaPg = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdTransacaoExterna = table.Column<string>(type: "TEXT", nullable: true),
                    RespostaPayload = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PontosFidelidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FidelizacaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoMovimentacaoPontos = table.Column<int>(type: "INTEGER", nullable: false),
                    Pontos = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataPontos = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontosFidelidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PontosFidelidade_Fidelidades_FidelizacaoId",
                        column: x => x.FidelizacaoId,
                        principalTable: "Fidelidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PontosFidelidade_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesMovimentacao_EstoqueId",
                table: "EstoquesMovimentacao",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesMovimentacao_PedidoId",
                table: "EstoquesMovimentacao",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesMovimentacao_UsuarioUsadoId",
                table: "EstoquesMovimentacao",
                column: "UsuarioUsadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesUnidade_EstabelecimentoId",
                table: "EstoquesUnidade",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesUnidade_ProdutoId",
                table: "EstoquesUnidade",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fidelidades_UsuarioId",
                table: "Fidelidades",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItensPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_ProdutoId",
                table: "ItensPedido",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_UsuarioId",
                table: "LogsAuditoria",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EstabelecimentoId",
                table: "Pedidos",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PontosFidelidade_FidelizacaoId",
                table: "PontosFidelidade",
                column: "FidelizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PontosFidelidade_PedidoId",
                table: "PontosFidelidade",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocoesCampanha_EstabelecimentoId",
                table: "PromocoesCampanha",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocoesCampanha_ProdutoId",
                table: "PromocoesCampanha",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesEstabelecimento_CNPJ",
                table: "UnidadesEstabelecimento",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CPF",
                table: "Usuarios",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EstabelecimentoId",
                table: "Usuarios",
                column: "EstabelecimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstoquesMovimentacao");

            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "PontosFidelidade");

            migrationBuilder.DropTable(
                name: "PromocoesCampanha");

            migrationBuilder.DropTable(
                name: "EstoquesUnidade");

            migrationBuilder.DropTable(
                name: "Fidelidades");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "UnidadesEstabelecimento");
        }
    }
}
