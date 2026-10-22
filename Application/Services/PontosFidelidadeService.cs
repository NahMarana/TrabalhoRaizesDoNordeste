using TrabalhoRaizesDoNordeste.API.Controllers;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;


namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PontosFidelidadeService(PontosFidelidadeRepository repository, UsuarioService usuarioService, FidelidadeService fidelidadeService)
    {
        public PontosDTO? BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pontos = repository.BuscaPontosPorId(id);

            if (pontos == null)
            {
                return null;
            }

            var pontosDTO = CriarObjeto(pontos);

            return pontosDTO;
        }

        public ICollection<PontosDTO> ListarPontosFidelidade()
        {
            var pontos = repository.ListarPontos();
            var pontosDTO = CriarObjetoCollection(pontos);

            return pontosDTO;
        }

        public void RegistrarPontuacao(PontosFidelidade pontos)
        {
            if (pontos.Pontos < 0)
            {
                throw new ArgumentException("A quantidade de pontos não pode ser negativa.");
            }

            if (!Enum.IsDefined(pontos.TipoMovimentacaoPontos))
            {
                throw new ArgumentException("Tipo de movimentação inválido.");
            }

            repository.CriarPontosFidelidade(pontos);
        }

        public PontosDTO CriarObjeto(PontosFidelidade pontos)
        {
            var pontosDTO = new PontosDTO
            {
                Id = pontos.Id,
                FidelizacaoId = pontos.FidelizacaoId,
                TipoMovimentacaoPontos = pontos.TipoMovimentacaoPontos.ToString(),
                Pontos = pontos.Pontos,
                DataPontos = pontos.DataPontos,
                Pedidos = pontos.Pedidos == null
                ? null : new PontosDoPedidoDTO
                {
                    Id = pontos.Pedidos.Id,
                    UsuarioId = pontos.Pedidos.UsuarioId,
                    ValorTotalPedido = pontos.Pedidos.ValorTotalPedido,
                }
            };
            return pontosDTO;
        }

        public ICollection<PontosDTO> CriarObjetoCollection(ICollection<PontosFidelidade> pontos)
        {
            var auxiliaPontosFidelidade = new List<PontosDTO>();

            foreach (var item in pontos)
            {
                auxiliaPontosFidelidade.Add(CriarObjeto(item));
            }

            return auxiliaPontosFidelidade;
        }
    }
}
