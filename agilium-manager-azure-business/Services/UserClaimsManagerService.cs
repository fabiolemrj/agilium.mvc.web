using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class UserClaimsManagerService : BaseService, IUserClaimsManagerService
    {
        private readonly IClaimRepository _claimRepository;
        public UserClaimsManagerService(INotificador notificador,
            IClaimRepository claimRepository) : base(notificador)
        {
            _claimRepository = claimRepository;
        }

        public async Task<bool> Adicionar(ObjetoClaim objetoClaim)
        {
            if (!ExecutarValidacao(new ObjetoClaimValidation(), objetoClaim)) return false;

            await _claimRepository.Adicionar(objetoClaim);

            return true;
        }

        public async Task<bool> Atualizar(ObjetoClaim objetoClaim)
        {
            if (!ExecutarValidacao(new ObjetoClaimValidation(), objetoClaim)) return false;

            await _claimRepository.Atualizar(objetoClaim);

            return true;
        }

        public void Dispose()
        {
            _claimRepository?.Dispose();
        }

        public async Task<ObjetoClaim> ObterClaimPorId(long id)
        {
            var claim = await _claimRepository.ObterPorId(id);
            return claim;
        }

        public async Task<ObjetoClaim> ObterClaimPorType(string typeClaim)
        {
            var claims = await _claimRepository.ObterTodos();
            return claims.FirstOrDefault(x => x.ClaimType == typeClaim);
        }

        public Task<ClaimValue> ObterClaimValuePOrValue(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> ObterListaClaim()
        {
            var claims = Enum.GetValues((typeof(EClaim)));
            var listaClaimConvertida = new List<string>();
            foreach (EClaim EClaim in (EClaim[])claims)
            {
                listaClaimConvertida.Add(EClaim.ToString());
            }
            return listaClaimConvertida;
        }

        public async Task<List<ClaimValue>> ObterListaClaimValues()
        {
            return await GerarListaClaimValue();
        }

        public async Task<List<ObjetoClaim>> ObterTodos()
        {
            var claims = Enum.GetValues((typeof(EClaim)));
            var listaClaimConvertida = new List<ObjetoClaim>();
            foreach (EClaim EClaim in (EClaim[])claims)
            {
                listaClaimConvertida.Add(new ObjetoClaim(EClaim.ToString()) );
            }
            return listaClaimConvertida;
            //return await _claimRepository.ObterTodos();
        }

        public async Task<bool> Remover(long id)
        {
            try
            {
                await _claimRepository.Remover(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<List<ClaimValue>> GerarListaClaimValue()
        {
            int _idGerado = 0;
            
            var listaClaimValue = new List<ClaimValue>();

            var claimValue = Enum.GetValues((typeof(EClaimValue)));
            var listaClaimValueConvertida = new List<ClaimValue>();
            foreach (EClaimValue eClaimValue in (EClaimValue[])claimValue)
            {
                listaClaimValueConvertida.Add(new ClaimValue(_idGerado++, eClaimValue.ToString()));
            }
            return listaClaimValueConvertida;

            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Incluir"));
            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Editar"));
            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Consultar"));
            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Remover"));
            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Relatorio"));
            //listaClaimValue.Add(new ClaimValue(_idGerado++, "Lista Excel"));
            //return listaClaimValue;
        }

    }
}
