using System.Threading.Tasks;

namespace agilum.mvc.web.Interfaces
{
    public interface IAutenticacaoService
    {
        bool TokenExpirado();
        Task<bool> RefreshTokenValido();
        Task Logout();
    }
}
