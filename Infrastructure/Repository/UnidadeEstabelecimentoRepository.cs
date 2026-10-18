using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class UnidadeEstabelecimentoRepository(AppDbContext _context)
    {
        public UnidadesEstabelecimento? BuscaUnidadePorId(int id)
        {
            return _context.UnidadesEstabelecimento
                .Include(eu => eu.EstoqueUnidades!).ThenInclude(e => e.Produtos).ThenInclude(p => p.Categoria)
                .Include(u => u.Usuarios)
                .FirstOrDefault(c => c.Id == id);
        }

        public ICollection<UnidadesEstabelecimento>? BuscarUnidadePorNome(string nome)
        {
            return _context.UnidadesEstabelecimento
                .Include(eu => eu.EstoqueUnidades!).ThenInclude(e => e.Produtos).ThenInclude(p => p.Categoria)
                .Include(u => u.Usuarios)
                .Where(ue => ue.NomeEstabelecimento.ToLower().Contains(nome.ToLower()))
                .ToList();
        }

        public ICollection<UnidadesEstabelecimento>? ListarUnidades()
        {
            return _context.UnidadesEstabelecimento
                .Include(eu => eu.EstoqueUnidades!).ThenInclude(e => e.Produtos).ThenInclude(p => p.Categoria)
                .Include(u => u.Usuarios)
                .ToList();
        }

        public ICollection<EstoqueUnidade>? ConsultarCardapio(int unidadeId)
        {
            var ProdutoSazonalEstaAtivo = (DateOnly.FromDateTime(DateTime.Today));
            return _context.EstoquesUnidade
                .Include(eu => eu.Produtos!)
                    .ThenInclude(p => p.Categoria)
                .Where(eu => eu.EstabelecimentoId == unidadeId
                          && eu.VendaDisponivel
                          && eu.QtdEstoqueDisponivel > 0
                          && eu.Produtos!.ProdutoAtivo
                          && (!eu.Produtos.ProdutoSazonal
                              || (eu.Produtos.DataInicioSazonal <= ProdutoSazonalEstaAtivo && eu.Produtos.DataFimSazonal >= ProdutoSazonalEstaAtivo))
                          && (eu.UnidadesEstabelecimento!.TipoUnidade == TipoUnidade.COMPLETA
                              || eu.Produtos.Categoria!.NomeCategoria != "Pratos Principais/PF"))
                .ToList();
        }

        public ICollection<PromocoesCampanhas> BuscaPromocoesAtivasDaUnidade(int unidadeId)
        {
            return _context.PromocoesCampanha
                .Where(pc => pc.EstabelecimentoId == unidadeId
                          && pc.StatusPromoCampanha == StatusPromoCampanha.ATIVO)
                .ToList();
        }

        public UnidadesEstabelecimento? CriarUnidade(UnidadesEstabelecimento unidade)
        {
            _context.UnidadesEstabelecimento.Add(unidade);
            _context.SaveChanges();

            return unidade;
        }

        public UnidadesEstabelecimento? AtualizarUnidade(int id, UnidadesEstabelecimento unidade)
        {
            var unidadeEncontrada = _context.UnidadesEstabelecimento.Find(id);

            if (unidadeEncontrada == null)
                return null;

            unidadeEncontrada.NomeEstabelecimento = unidade.NomeEstabelecimento;
            unidadeEncontrada.CNPJ = unidade.CNPJ;
            unidadeEncontrada.UnidadeAtiva = unidade.UnidadeAtiva;
            unidadeEncontrada.TipoUnidade = unidade.TipoUnidade;
            unidadeEncontrada.Rua = unidade.Rua;
            unidadeEncontrada.Numero = unidade.Numero;
            unidadeEncontrada.Bairro = unidade.Bairro;
            unidadeEncontrada.Cidade = unidade.Cidade;
            unidadeEncontrada.Estado = unidade.Estado;
            unidadeEncontrada.CEP = unidade.CEP;
            unidadeEncontrada.TelefoneUnidade = unidade.TelefoneUnidade;


            _context.UnidadesEstabelecimento.Update(unidadeEncontrada);
            _context.SaveChanges();

            return unidadeEncontrada;
        }

        public UnidadesEstabelecimento? DesativarUnidade(int id)
        {
            var unidade = _context.UnidadesEstabelecimento.Find(id);

            if (unidade == null)
                return null;

            unidade.UnidadeAtiva = false;

            _context.UnidadesEstabelecimento.Update(unidade);
            _context.SaveChanges();

            return unidade;
        }

        public bool Deletar(int id)
        {
            var unidade = _context.UnidadesEstabelecimento.Find(id);

            if (unidade == null)
                return false;

            _context.UnidadesEstabelecimento.Remove(unidade);
            var response = _context.SaveChanges();

            return response > 0;
        }
    }
}
