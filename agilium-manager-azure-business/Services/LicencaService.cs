using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium_manager_azure_business.Interfaces.IService;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using agilium.api.business.Services;
using System.Linq;
using agilium.api.business.Models;
using agilium_manager_azure_business.Services;

namespace agilium.api.business.Services
{
    public class LicencaService : BaseService, ILicencaService
    {
        private readonly ILicencaRepository _licencaRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        public LicencaService(INotificador notificador, ILicencaRepository licencaRepository, IUtilDapperRepository utilDapperRepository) : base(notificador)
        {
            _licencaRepository = licencaRepository;
            _utilDapperRepository = utilDapperRepository ;
        }

        public void Dispose()
        {
            _licencaRepository?.Dispose();
        }

        public async Task<Licenca> ObterPorIdEmpresa(string idLicenca, string idEmpresa)
        {
            return _licencaRepository.Obter(x => x.IDEMPRESA == Convert.ToInt64(idEmpresa)).Result.FirstOrDefault();
        }

        public async Task<bool> DataValida(long idEmpresa)
        {
            var objeto = await ObterPorIdEmpresa("0", idEmpresa.ToString());

            var licencaDataValidade = Descriptografar(objeto.K5);
            return Convert.ToDateTime(licencaDataValidade) >= await _utilDapperRepository.ObterDataAtual();
        }

        public string Descriptografar(string value)
        {
            // Cria uma nova instância da classe PassCryptoService.
            // O uso de `null` no Delphi `Create(nil)` indica que o componente
            // não tem um "owner" (proprietário), que é o comportamento padrão no C#.
            PassCryptoService cryptoService = null;
            try
            {
                cryptoService = new PassCryptoService();

                // Define a senha e o modo de operação.
                cryptoService.PassWord = value;
                cryptoService.Mode = FunctionMode.Decode;

                // Retorna o resultado da descriptografia.
                return cryptoService.PasswrdCrypto();
            }
            finally
            {
                // A instrução `x.free()` do Delphi é uma forma de liberar a memória.
                // No C#, o coletor de lixo gerencia a memória, então a
                // chamada `Dispose()` (ou `free()` no Delphi) não é necessária a menos
                // que a classe `PassCryptoService` implemente `IDisposable`.
                // A instrução `try...finally` garante que o objeto seja liberado,
                // embora não seja estritamente necessário para esta classe.
            }
        }
    }
}
