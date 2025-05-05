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
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteContatoRepository _clienteContatoRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IClientePFRepository _clientePFRepository;
        private readonly IClientePJRepository _clientePJRepository;
        private readonly IClientePrecoRepository _clientePrecoRepository;
        private readonly IClienteDapperRepository _clienteDapperRepository; 
        private readonly IDapperRepository _dapperRepository;

        public ClienteService(INotificador notificador, IClienteRepository clienteRepository, 
            IClienteContatoRepository clienteContatoRepository, IContatoRepository contatoRepository,
            IClientePFRepository clientePFRepository, IClientePJRepository clientePJRepository, IClientePrecoRepository clientePrecoRepository,
            IClienteDapperRepository clienteDapperRepository, IDapperRepository dapperRepository) : base(notificador)
        {
            _clienteRepository = clienteRepository;
            _clienteContatoRepository = clienteContatoRepository;
            _contatoRepository = contatoRepository;
            _clientePFRepository = clientePFRepository;
            _clientePJRepository = clientePJRepository;
            _clientePrecoRepository = clientePrecoRepository;
            _clienteDapperRepository = clienteDapperRepository;
            _dapperRepository = dapperRepository;
        }


        public void Dispose()
        {
            _clienteRepository?.Dispose();
            _clienteContatoRepository?.Dispose();
            _contatoRepository?.Dispose();
            _clientePrecoRepository.Dispose();
            _clientePFRepository.Dispose();
            _clientePJRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _clienteRepository.SaveChanges();
        }

        #region Cliente
        public async Task Adicionar(Cliente cliente)
        {
            if (!ExecutarValidacao(new ClienteValidation(), cliente))
                return;

            await _clienteRepository.AdicionarSemSalvar(cliente);
        }

        public async Task Atualizar(Cliente cliente)
        {
            if (!ExecutarValidacao(new ClienteValidation(), cliente))
                return;

            await _clienteRepository.AtualizarSemSalvar(cliente);
        }

        public async Task<Cliente> ObterCompletoPorId(long id)
        {
            var lista = _clienteRepository.Obter(x => x.Id == id, "Endereco", "ClienteContatos", "ClienteContatos.Contato",
                "ClientesPFs", "ClientesPJs", "EnderecoCobranca", "EnderecoFaturamento", "EnderecoEntrega").Result;
            
            return lista.FirstOrDefault();
        }

        public async Task<Cliente> ObterPorId(long id)
        {
            return _clienteRepository.ObterPorId(id).Result;
        }

        public async Task<List<Cliente>> ObterPorNome(string descricao)
        {
            return _clienteRepository.Buscar(x => x.NMCLIENTE.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Cliente>> ObterPorNomePaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _clienteRepository.Buscar(x => x.NMCLIENTE.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Cliente>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<List<Cliente>> ObterTodos()
        {
            return _clienteRepository.ObterTodos().Result;
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagarCliente(id).Result)
            {
                Notificar("Não foi possível apagar o cliente, pois este possui historico!");
                return;
            }
            await _clienteRepository.RemoverSemSalvar(id);
        }

        #endregion

        #region Contato
        public async Task AdicionarContato(ClienteContato clienteContato)
        {

            await _clienteContatoRepository.AdicionarSemSalvar(clienteContato);
        }

        public async Task<ClienteContato> ObterClienteContatoPorId(long idCliente, long idContato)
        {
            return _clienteContatoRepository.Buscar(x => x.IDCONTATO == idContato && x.IDCLIENTE == idCliente, "Contato").Result.FirstOrDefault();
        }

        public async Task<List<ClienteContato>> ObterFornecedoresContatosPorFornecedor(long idCliente)
        {
            return _clienteContatoRepository.Buscar(x => x.IDCLIENTE == idCliente).Result.ToList();
        }

        public async Task RemoverContato(ClienteContato clienteContato)
        {
            await _clienteContatoRepository.RemoverSemSalvar(clienteContato);

            //remover contato
            var contato = ObterContatoPorId(clienteContato.IDCONTATO).Result;
            if (contato != null)
                await _contatoRepository.RemoverSemSalvar(contato);
        }

        public async Task RemoverContato(long idCliente, long idContato)
        {
            var clienteContato = _clienteContatoRepository.Obter(x => x.IDCONTATO == idContato && x.IDCLIENTE == idCliente).Result.FirstOrDefault();
            if (clienteContato != null)
                await RemoverContato(clienteContato);
        }

        public async Task Atualizar(ClienteContato clienteContato)
        {
            await _clienteContatoRepository.AtualizarSemSalvar(clienteContato);
        }
        #endregion

        #region Metodos Privados
        private async Task<bool> PodeApagarCliente(long idFornecedor) => false;

        private async Task<Contato> ObterContatoPorId(long idContato)
        {
            return _contatoRepository.ObterPorId(idContato).Result;
        }

        private async Task<bool> PodeApagarClientePrecoPorId(long idContato)
        {
            return true;
        }
        #endregion

        #region ClientePF
        public async Task Adicionar(ClientePF cliente)
        {
            await _clientePFRepository.AdicionarSemSalvar(cliente);
        }

        public async Task Atualizar(ClientePF cliente)
        {
            await _clientePFRepository.AtualizarSemSalvar(cliente);
        }
        #endregion

        #region ClientePF
        public async Task Adicionar(ClientePJ cliente)
        {
            await _clientePJRepository.AdicionarSemSalvar(cliente);
        }

        public async Task Atualizar(ClientePJ cliente)
        {
            await _clientePJRepository.AtualizarSemSalvar(cliente);
        }
        #endregion

        #region Cliente Preço
        public async Task Adicionar(ClientePreco cliente)
        {
            if (!ExecutarValidacao(new ClientePrecoValidation(), cliente))
                return;

            await _clientePrecoRepository.AdicionarSemSalvar(cliente);
        }

        public async Task Remover(long id)
        {
            if (!PodeApagarClientePrecoPorId(id).Result)
            {
                Notificar("Não foi possível apagar o Preço do produto para o cliente, pois este possui historico!");
                return;
            }
            await _clientePrecoRepository.RemoverSemSalvar(id);
        }

        public async Task<ClientePreco> ObteClientePrecoPorId(long id)
        {
            return _clientePrecoRepository.ObterPorId(id).Result;
        }

        public async Task<IEnumerable<ClientePreco>> ObterClientePrecoPorProduto(long idProduto)
        {
            return _clientePrecoRepository.Obter(x=>x.IDPRODUTO == idProduto).Result;
        }

        #endregion

        #region Dapper

        public async Task<Cliente> ObterClientePorId(long id)
        {
            return await _clienteDapperRepository.ObterClientePorId(id);
        }

        public async Task<long> AdicionarClienteBasico(Cliente cliente, string cpf)
        {
            var resultado = 0L;
            try
            {
                await _dapperRepository.BeginTransaction();

                var novoCliente = await _clienteDapperRepository.AdicionarClienteBasico(cliente);

                if(novoCliente > 0)
                {
                    var clientePF = new ClientePF(cpf, null, null);
                    clientePF.AdicionarIdCliente(novoCliente);

                    if(!await _clienteDapperRepository.AdicionarClientePF(clientePF))
                    {
                        Notificar("Erro ao tenta adicionar cliente PF");
                    }
                }
                else
                {
                    Notificar("Erro ao tentar adicionar cliente");
                }

                if (!TemNotificacao())
                {
                    resultado = novoCliente;
                 
                    await _dapperRepository.Commit();

                }
                else
                {
                    resultado = 0;
                    Notificar("Erro ao salvar novo cliente");
                    await _dapperRepository.Rollback();
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                Notificar(ex.Message);
            }

            return resultado;
        }

        public async Task<Cliente> ObterClienteComEnderecoPorId(long id)
        {
            return await _clienteDapperRepository.ObterClienteComEnderecoPorId(id);
        }
        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            return await _clienteDapperRepository.ObterClientePorCpf(cpf);
        }


        #endregion
    }
}
