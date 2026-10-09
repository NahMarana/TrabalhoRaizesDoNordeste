using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class PontosFidelidadeRepository(AppDbContext _context)
    {
        public PontosFidelidade? BuscaPontosPorId(int id)
        {
            return _context.PontosFidelidade
                .Include(p => p.Fidelidade)
                .Include(pf => pf.Pedidos)
                .FirstOrDefault(pf => pf.Id == id);
        }

        public ICollection<PontosFidelidade> ListarPontos()
        {
            return _context.PontosFidelidade
                .Include(pf => pf.Fidelidade)
                .Include(pf => pf.Pedidos)
                .ToList();
        }

        public Fidelidade? BuscaFidelidadePorId(int fidelizacaoId)
        {
            return _context.Fidelidades.Find(fidelizacaoId);
        }

        public Pedido? BuscaPedidoPorId(int pedidoId)
        {
            return _context.Pedidos.Find(pedidoId);
        }

        public PontosFidelidade? CriarPontosFidelidade(PontosFidelidade pontos)
        {
            _context.PontosFidelidade.Add(pontos);
            _context.SaveChanges();

            RegistarPontuacao(pontos);

            return pontos;
        }

        public void RegistarPontuacao(PontosFidelidade pontos)
        {
            var fidelidade = _context.Fidelidades.Find(pontos.FidelizacaoId);

            if (fidelidade == null)
                return;

            if (pontos.TipoMovimentacaoPontos == TipoMovimentoPontos.ACUMULO)
            {
                fidelidade.QtdPontos += pontos.Pontos;
            }
            else if (pontos.TipoMovimentacaoPontos == TipoMovimentoPontos.RESGATE)
            {
                fidelidade.QtdPontos -= pontos.Pontos;

                if (fidelidade.QtdPontos < 0)
                    fidelidade.QtdPontos = 0;
            }

            _context.Fidelidades.Update(fidelidade);
            _context.SaveChanges();
        }

        public bool PossuiPontuacao(int pedidoId)
        {
            return _context.PontosFidelidade.Any(p =>
                p.PedidoId == pedidoId &&
                p.TipoMovimentacaoPontos == TipoMovimentoPontos.ACUMULO);
        }
    }
}
