using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace agilium.webapp.manager.mvc.Services
{
    public class ImportarXmlNfeService : Service, IImportarXMLNfe
    {
        public ImportarXmlNfeService(HttpClient httpClient, IOptions<AppSettings> settings, IAspNetUser user, IAuthenticationService authenticationService, IConfiguration configuration) : base(httpClient, settings, user, authenticationService, configuration)
        {
        }

        public async Task<NFeProc> LerXML(string caminhoArquivo)
        {
            var nfe = GetObjectFromFile<NFeProc>(caminhoArquivo);

            return nfe;
        }

        //public async Task<bool> LerXML(string caminhoArquivo)
        //{
        //    var nfe = GetObjectFromFile<NFeProc>(caminhoArquivo);

        //    if(nfe == null) return false;

        //    return true;
        //}

        public async Task<ResponseResult> LerXMLNFE(string caminhoArquivo)
        {
            var nfe = GetObjectFromFile<NFeProc>(caminhoArquivo);

            if (nfe == null)
            {
                return RetornoBadRequest(new { Data = "Falha ao ler o arquivo xml. Verifique se o arquivo é de uma NF-e/NFC-e autorizada!"});
            }

            return RetornoOk("Nota Fiscal Convertida");
        }

        private T GetObjectFromFile<T>(string arquivo) where T : class
        {
            var serialize = new XmlSerializer(typeof(T));

            try
            {
                var xmlArquivo = System.Xml.XmlReader.Create(arquivo);
                return (T)serialize.Deserialize(xmlArquivo);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
