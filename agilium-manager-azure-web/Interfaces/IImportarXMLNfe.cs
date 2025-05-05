
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IImportarXMLNfe
    {
       // Task<bool> LerXML(string caminhoArquivo);
        Task<ResponseResult> LerXMLNFE(string caminhoArquivo);
        Task<NFeProc> LerXML(string caminhoArquivo);
    }
}
