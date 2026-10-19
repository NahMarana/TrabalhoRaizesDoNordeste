using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class PromocoesCampanhasRepository(AppDbContext _context)
    {
        public PromocoesCampanhas? BuscaPromocoesCampanhaPorId(int id)
        {
            return _context.PromocoesCampanha
                .Include(pc => pc.UnidadesEstabelecimento)
                .Include(pc => pc.Produtos)
                .FirstOrDefault(pc => pc.Id == id);
        }

        public ICollection<PromocoesCampanhas>? ListarPromocoesCampanhasAtivas()
        {
            return _context.PromocoesCampanha
                .Include(pc => pc.UnidadesEstabelecimento)
                .Include(pc => pc.Produtos)
                .Where(pc => pc.StatusPromoCampanha == StatusPromoCampanha.ATIVO)
                .ToList();
        }

        public ICollection<PromocoesCampanhas>? ListarPromocoesCampanhasInativas()
        {
            return _context.PromocoesCampanha
                .Include(pc => pc.UnidadesEstabelecimento)
                .Include(pc => pc.Produtos)
                .Where(pc => pc.StatusPromoCampanha == StatusPromoCampanha.FINALIZADO)
                .ToList();
        }

        public Produto? BuscaProdutoPorId(int produtoId)
        {
            return _context.Produtos.Find(produtoId);
        }

        public UnidadesEstabelecimento? BuscaEstabelecimentoPorId(int estabelecimentoId)
        {
            return _context.UnidadesEstabelecimento.Find(estabelecimentoId);
        }

        public PromocoesCampanhas CriarPromoCampanha(PromocoesCampanhas promo)
        {
            _context.PromocoesCampanha.Add(promo);
            _context.SaveChanges();

            return promo;
        }

        public PromocoesCampanhas? AtualizarPromoCampanha(int id, PromocoesCampanhas promo)
        {
            var promoEncontrada = _context.PromocoesCampanha.Find(id);

            if (promoEncontrada == null)
                return null;

            promoEncontrada.ValorPromocional = promo.ValorPromocional;
            promoEncontrada.Descricao = promo.Descricao;
            promoEncontrada.StatusPromoCampanha = promo.StatusPromoCampanha;
            promoEncontrada.DataFim = promo.DataFim;

            _context.PromocoesCampanha.Update(promoEncontrada);
            _context.SaveChanges();

            return promoEncontrada;
        }

        public PromocoesCampanhas? DesativarPromo(int id)
        {
            var promo = _context.PromocoesCampanha.Find(id);

            if (promo == null)
                return null;

            promo.StatusPromoCampanha = StatusPromoCampanha.FINALIZADO;

            _context.PromocoesCampanha.Update(promo);
            _context.SaveChanges();

            return promo;
        }

        public void FinalizarPromoVencidas()
        {
            var datePromo = DateOnly.FromDateTime(DateTime.Today);

            var vencidas = _context.PromocoesCampanha
                .Where(pc => pc.StatusPromoCampanha == StatusPromoCampanha.ATIVO && pc.DataFim < datePromo)
                .ToList();

            foreach (var promo in vencidas)
            {
                promo.StatusPromoCampanha = StatusPromoCampanha.FINALIZADO;
            }

            _context.SaveChanges();
        }
    }
}
