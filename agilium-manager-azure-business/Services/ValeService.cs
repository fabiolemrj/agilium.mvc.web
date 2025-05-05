using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class ValeService : BaseService, IValeService
    {
        private readonly IValeRepository _valeRepository;
        private readonly IValeDapperRepository _valeDapperRepository;
        public ValeService(INotificador notificador, IValeRepository valeRepository, IValeDapperRepository valeDapperRepository) : base(notificador)
        {
            _valeRepository = valeRepository;
            _valeDapperRepository = valeDapperRepository;
        }

        public async Task Salvar()
        {
            await _valeRepository.SaveChanges();
        }

        public void Dispose()
        {
            _valeRepository?.Dispose();
        }

        public async Task Adicionar(Vale vale)
        {
            if (!ExecutarValidacao(new ValeValidation(), vale))
                return;

            await _valeRepository.AdicionarSemSalvar(vale);
        }

        public async Task Apagar(long id)
        {
            if (await ValeNaoPodeSerExcluido(id))
            {
                Notificar("Não é possivel apagar este vale pois o mesmo está sendo utilizado");
                return;
            }
            await _valeRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Vale vale)
        {
            if (!ExecutarValidacao(new ValeValidation(), vale))
                return;

            await _valeRepository.AtualizarSemSalvar(vale);
        }

        public async Task<Vale> ObterPorId(long id)
        {
            return await _valeRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Vale>> ObterTodas(long idEmpresa)
        {
            return await _valeRepository.Obter(x=>x.IDEMPRESA == idEmpresa);
        }

        public async Task<PagedResult<Vale>> ObterValePorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _valeRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.Cliente.NMCLIENTE.ToUpper().Contains(_nomeParametro.ToUpper()), "Cliente");

            return new PagedResult<Vale>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task CancelarVale(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<long> GerarVale(long idEmpresa, long? idCliente, ETipoVale tipoVale, double valor)
        {
            var codigo = GerarCodigoVale(idEmpresa).Result;
            var codigoBarra = GerarCodigoBarraVale().Result;
            var vale = new Vale(idEmpresa,idCliente,codigo,DateTime.Now,ETipoVale.Troca,ESituacaoVale.Ativo,valor,codigoBarra);

            await _valeRepository.AdicionarSemSalvar(vale);

            return vale.Id;
        }

        public async Task<long> GerarVale(long idDevolucao)
        {
           return await _valeDapperRepository.GerarValeAtualizaDevolucao(idDevolucao);
        }

        public async Task<bool> UtilizarValePorVenda(long idVenda)
        {
            return await _valeDapperRepository.UtilizarValePorVenda(idVenda);
        }



        #region private

        private async Task<string> GerarCodigoVale(long idEmpresa)
        {
            var vales = ObterTodas(idEmpresa).Result;

            var result = vales.Max(x => x.CDVALE);
            var codigoConvertido = Convert.ToInt32(result);

            return (codigoConvertido+1).ToString().PadLeft(6,'0');
        }
        private async Task<bool> ValeNaoPodeSerExcluido(long id) => false;

        public async Task<string> GerarCodigoBarraVale()
        {
            var resultado = string.Empty;
            var cdBarraUnico = false;
            var quant = 0;
            var numeroRandomico = 0;

            Random valor = new Random();
            do
            {
                numeroRandomico = valor.Next(9999) + 1;
                quant++;

                resultado = DateTime.Now.ToString("yyMMdd") + numeroRandomico.ToString().PadLeft(4, '0');
                cdBarraUnico = _valeRepository.Obter(x => x.CDBARRA.Trim().ToUpper() == resultado.Trim().ToUpper()).Result.Any();
            } while (cdBarraUnico || quant > 100);

            return resultado;
        }


        #endregion
    }
}
