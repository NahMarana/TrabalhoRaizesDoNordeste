namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class EstoqueUnidade
    {
        public int Id { get; set; }
        public required int ProdutoId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required int QtdEstoqueDisponivel { get; set; }
        public required bool VendaDisponivel { get; set; }
        public Produto? Produtos { get; set; }
        public UnidadesEstabelecimento? UnidadesEstabelecimento { get; set; }
        public ICollection<EstoqueMovimentacao>? EstoquesMovimentacao { get; set; }

        // Vai conferir se a venda do produto esta disponivel no estoque da unidade e se tem quantidade suficiente 
        public bool PossuiEstoqueSuficiente(int qtdDisponivel)
        {
            return VendaDisponivel && QtdEstoqueDisponivel >= qtdDisponivel;

        }

        //Saída de estoque(DAR BAIXA) = diminui a quantidade no estoque
        public void DarBaixaEstoque(int qtdDisponivel)
        {
            if (qtdDisponivel <= 0)
            {
                throw new ArgumentException("É obrigatório ter pelo menos um produto.");
            }

            if (!VendaDisponivel)
            {
                throw new InvalidOperationException("Produto indisponível para venda nesta unidade.");
            }

            if (QtdEstoqueDisponivel < qtdDisponivel)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {QtdEstoqueDisponivel}");
            }
            QtdEstoqueDisponivel -= qtdDisponivel;
        }

        //Entrada de estoque(REPOR ESTOQUE) = aumenta ou devolver a quantidade ao estoque
        public void ReporEstoque(int qtdDisponivel)
        {
            QtdEstoqueDisponivel += qtdDisponivel;
        }
    }
}
