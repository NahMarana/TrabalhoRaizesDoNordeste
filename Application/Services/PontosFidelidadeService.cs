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
            if (id < 0)
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

        public PontosFidelidade CriarPontos(PontosFidelidade pontos)
        {
            if (pontos == null)
            {
                throw new ArgumentNullException("Pontos por Fidelidade inexistente.");
            }

            if (pontos.FidelizacaoId == null || pontos.FidelizacaoId < 0)
            {
                throw new ArgumentNullException("ID da Fidelização inexistente.");
            }
            
            var pontosCriados = repository.CriarPontosFidelidade(pontos);

            return pontosCriados;
        }

        public PontosDTO CriarObjeto(PontosFidelidade pontos)
        {
            var pontosDTO = new PontosDTO
            {
                Id = pontos.Id,
                FidelizacaoId = pontos.FidelizacaoId,
                TipoMovimentacaoPontos = pontos.TipoMovimentacaoPontos,
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
                var pontosDTO = new PontosDTO
                {
                    Id = item.Id,
                    FidelizacaoId = item.FidelizacaoId,
                    TipoMovimentacaoPontos = item.TipoMovimentacaoPontos,
                    Pontos = item.Pontos,
                    DataPontos = item.DataPontos,
                    Pedidos = item.Pedidos == null
                ? null : new PontosDoPedidoDTO
                {
                    Id = item.Pedidos.Id,
                    UsuarioId = item.Pedidos.UsuarioId,
                    ValorTotalPedido = item.Pedidos.ValorTotalPedido,
                }
                };

                auxiliaPontosFidelidade.Add(pontosDTO);
            }

            return auxiliaPontosFidelidade;
        }

        public void ConsumirSaldo(PontosFidelidade fidelidade)
        {
            if (fidelidade.TipoMovimentacaoPontos == TipoMovimentoPontos.ACUMULO) 
            {

                //metodo para acumular pontos
                return;
            }

            //meotodo para resgatar pontos
        }
    }
}
