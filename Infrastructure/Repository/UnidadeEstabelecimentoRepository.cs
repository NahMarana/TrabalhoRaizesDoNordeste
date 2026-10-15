using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
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

        public UnidadesEstabelecimento? BuscarUnidadePorNome(string nome)
        {
            return _context.UnidadesEstabelecimento
                .Include(eu => eu.EstoqueUnidades!).ThenInclude(e => e.Produtos).ThenInclude(p => p.Categoria)
                .Include(u => u.Usuarios)
                .FirstOrDefault(ue => ue.NomeEstabelecimento.ToLower().Contains(nome.ToLower()));
        }

        public ICollection<UnidadesEstabelecimento>? ListarUnidades()
        {
            return _context.UnidadesEstabelecimento
                .Include(eu => eu.EstoqueUnidades)
                .Include(u => u.Usuarios)
                .ToList();
        }

        public ICollection<EstoqueUnidade>? ConsultarCardapio(int unidadeId)
        {
            return _context.EstoquesUnidade
                .Include(eu => eu.Produtos!)
                    .ThenInclude(p => p.Categoria)
                .Where(eu => eu.EstabelecimentoId == unidadeId
                          && eu.VendaDisponivel
                          && eu.QtdEstoqueDisponivel > 0
                          && eu.Produtos!.ProdutoAtivo)
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
    }
}
