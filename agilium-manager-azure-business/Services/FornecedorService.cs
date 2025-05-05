using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepsoitory _fornecedorRepsoitory;
        private readonly IFornecedorContatoRepsoitory _fornecedorContatoRepository;
        private readonly IContatoRepository _contatoRepository;
        public FornecedorService(INotificador notificador, IFornecedorContatoRepsoitory contatoRepsoitory,
                                    IFornecedorRepsoitory fornecedorRepsoitory, IContatoRepository contatoRepository) : base(notificador)
        {
            _fornecedorContatoRepository = contatoRepsoitory;
            _fornecedorRepsoitory = fornecedorRepsoitory;
            _contatoRepository = contatoRepository;
        }

        #region Fornecedor
        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor))
                return;

            await _fornecedorRepsoitory.AdicionarSemSalvar(fornecedor);
        }

        public async Task Apagar(long id)
        {
            if(!PodeApagarFornecedor(id).Result)
            {
                Notificar("Não foi possível apagar o fornecedor, pois este possui historico com produtos e movimentações de estoques!");
                return;
            }
            await _fornecedorRepsoitory.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor))
                return;

            await _fornecedorRepsoitory.AtualizarSemSalvar(fornecedor);
        }

        public async Task<Fornecedor> ObterCompletoPorId(long id)
        {
            var lista = _fornecedorRepsoitory.Obter(x => x.Id == id, "Endereco", "FornecedoresContatos", "FornecedoresContatos.Contato").Result;
            return lista.FirstOrDefault();
        }

        public async Task<Fornecedor> ObterPorId(long id)
        {
            return _fornecedorRepsoitory.ObterPorId(id).Result;
        }

        public async Task<List<Fornecedor>> ObterPorRazaoSocial(string descricao)
        {
            return _fornecedorRepsoitory.Buscar(x => x.NMRZSOCIAL.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Fornecedor>> ObterPorRazaoSocialPaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _fornecedorRepsoitory.Buscar(x => x.NMRZSOCIAL.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Fornecedor>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<List<Fornecedor>> ObterTodos()
        {
            return _fornecedorRepsoitory.ObterTodos().Result;
        }
        #endregion

        #region ContatoFornecedor
        public async Task AdicionarContato(FornecedorContato fornecedorContato)
        {
            await _fornecedorContatoRepository.AdicionarSemSalvar(fornecedorContato);
        }

        public async Task<FornecedorContato> ObterFornecedorContatoPorId(long idFornecedor, long idContato)
        {
            return _fornecedorContatoRepository.Buscar(x => x.IDCONTATO == idContato && x.IDFORN == idFornecedor,"Contato").Result.FirstOrDefault();
        }

        public async Task<List<FornecedorContato>> ObterFornecedoresContatosPorFornecedor(long idFornecedor)
        {
            return _fornecedorContatoRepository.Buscar(x => x.IDFORN == idFornecedor).Result.ToList();
        }

        public async Task RemoverContato(FornecedorContato fornecedorContato)
        {
            await _fornecedorContatoRepository.RemoverSemSalvar(fornecedorContato);

            //remover contato
            var contato = ObterContatoPorId(fornecedorContato.IDCONTATO).Result;
            if(contato != null)
                await _contatoRepository.RemoverSemSalvar(contato);
        }

        public async Task RemoverContato(long idFornecedor, long idContato)
        {
            var contatoFornecedor = _fornecedorContatoRepository.Obter(x => x.IDCONTATO == idContato && x.IDFORN == idFornecedor).Result.FirstOrDefault();
            if(contatoFornecedor != null)
                await RemoverContato(contatoFornecedor);
        }

        public async Task Atualizar(FornecedorContato fornecedorContato)
        {
            await _fornecedorContatoRepository.AtualizarSemSalvar(fornecedorContato);
        }
        #endregion

        #region Metodos Privados
        private async Task<bool> PodeApagarFornecedor(long idFornecedor) => false;

        private async Task<Contato> ObterContatoPorId(long idContato)
        {
            return _contatoRepository.ObterPorId(idContato).Result;
        }

        #endregion
            
        public void Dispose()
        {
            _fornecedorRepsoitory?.Dispose();
            _fornecedorContatoRepository?.Dispose();
        }  

        public async Task Salvar()
        {
            await _fornecedorRepsoitory.SaveChanges();
        }

        
    }
}
