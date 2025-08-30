using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;

using System.Threading.Tasks;

namespace agilum.mvc.web.Interfaces
{
    public interface IImportarXMLNfe
    {
        // Task<bool> LerXML(string caminhoArquivo);
        Task<bool> LerXMLNFE(string caminhoArquivo);
        Task<NFeProc> LerXML(string caminhoArquivo);
    }
}
