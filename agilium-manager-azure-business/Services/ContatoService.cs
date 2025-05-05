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
    public class ContatoService : BaseService, IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IContatoEmpresaRepository _contatoEmpresaRepository;
        

        public ContatoService(IContatoRepository contatoRepository, IContatoEmpresaRepository contatoEmpresaRepository,
            INotificador notificador) : base(notificador)
        {
            _contatoRepository = contatoRepository;
            _contatoEmpresaRepository = contatoEmpresaRepository;
        }

        public async Task Adicionar(Contato contato)
        {
            if (!ExecutarValidacao(new ContatoValidation(), contato))
                return;

            await _contatoRepository.AdicionarSemSalvar(contato);
        }

        public async Task Adicionar(ContatoEmpresa contatoEmpresa)
        {
            await _contatoEmpresaRepository.AdicionarSemSalvar(contatoEmpresa);
        }

        public async Task Apagar(long id)
        {
            if(await ExisteContatoUtilizado(id))
            {
                Notificar("Não é possivel apagar este contato pois o mesmo está sendo utilizado (empresa)");
                return;
            }
            await _contatoRepository.RemoverSemSalvar(id);
        }

        public async Task Apagar(long idContato, long idEmpresa)
        {
            var contatoEmpresa = _contatoEmpresaRepository.Obter(x => x.IDCONTATO == idContato && x.IDEMPRESA == idEmpresa).Result.FirstOrDefault();
            if(contatoEmpresa == null)
            {
                Notificar("contato empresa não localizado");
                return;
            }
            await _contatoEmpresaRepository.RemoverSemSalvar(contatoEmpresa);
        }

        public async Task Atualizar(Contato contato)
        {
            if (!ExecutarValidacao(new ContatoValidation(), contato))
                return;

            await _contatoRepository.AtualizarSemSalvar(contato);
        }

        public async Task Atualizar(ContatoEmpresa contatoEmpresa)
        {
            await _contatoEmpresaRepository.AtualizarSemSalvar(contatoEmpresa);
        }

        public void Dispose()
        {
            _contatoEmpresaRepository?.Dispose();
            _contatoRepository?.Dispose();
        }

        public async Task<Contato> ObterPorId(long id)
        {
            return await _contatoRepository.ObterPorId(id);
        }

        public async Task<ContatoEmpresa> ObterPorId(long idContato, long idEmpresa)
        {
            return _contatoEmpresaRepository.Obter(x => x.IDCONTATO == idContato && x.IDEMPRESA == idEmpresa, "Contato").Result.FirstOrDefault();
        }

        public async Task Salvar()
        {
            await _contatoRepository.SaveChanges() ;
        }

        #region private
        private async Task<bool> ExisteContatoUtilizado(long idContato)
        {
            var resultado = false;
            resultado = _contatoEmpresaRepository.Obter(x => x.IDCONTATO == idContato).Result.Any();
            
            return resultado;
        }
        #endregion
    }
}
