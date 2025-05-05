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
    public class EnderecoService : BaseService, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICepRepository _cepRepository;

        public EnderecoService(INotificador notificador, IEnderecoRepository enderecoRepository,
             ICepRepository cepRepository) : base(notificador)
        {
            _enderecoRepository = enderecoRepository;
            _cepRepository = cepRepository;
        }

        public async Task<bool> AdicionarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return false;

            await _enderecoRepository.AdicionarSemSalvar(endereco);
            return true;
        }

        public async Task<bool> ApagarEndereco(long id)
        {
            await _enderecoRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<bool> AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return false;

            await _enderecoRepository.AtualizarSemSalvar(endereco);
            return true;
        }

        public void Dispose()
        {
            _enderecoRepository?.Dispose();
        }

        public async Task<Cep> ObterCepPorNumeroCep(string cep)
        {
            return _cepRepository.Buscar(x => x.Numero.ToLower() == cep.ToLower().Replace(".", "").Replace("-", "")).Result.FirstOrDefault();
        }

        public async Task<Endereco> ObterEnderecoPorId(long id)
        {
            return await _enderecoRepository.ObterPorId(id);
        }

        public async Task Salvar()
        {
            await _enderecoRepository.SaveChanges();
        }

        public async Task<bool> AtualizarAdicionar(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return false;

            var enderecoExistente = _enderecoRepository.Existe(x=>x.Id == endereco.Id).Result;
            if (!enderecoExistente)
                await _enderecoRepository.AdicionarSemSalvar(endereco);
            else
                await _enderecoRepository.AtualizarSemSalvar(endereco);
            return true;
        }
    }
}
