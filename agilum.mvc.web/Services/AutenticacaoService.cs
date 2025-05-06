using agilum.mvc.web.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace agilum.mvc.web.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {


        public AutenticacaoService()
        {            
        }

        public async Task Logout()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RefreshTokenValido()
        {
            throw new System.NotImplementedException();
        }

        public bool TokenExpirado()
        {
            throw new System.NotImplementedException();
        }
    }
}
