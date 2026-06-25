using Microsoft.AspNetCore.Identity;
using System;

namespace agilium.api.business.Models
{
    /// <summary>
    /// Classe Identity para login, vinculada a entidade de dominio Usuario (ca_usuarios).
    /// Herda IdentityUser e serve APENAS para autenticacao/autorizacao.
    /// O Id (string/GUID) e vinculado via Usuario.idUserAspNet a entidade Usuario.
    /// Nao possui metodos complexos de sincronizacao - e mantida simples propositalmente.
    /// </summary>
    public class CaUsuarioIdentity : IdentityUser
    {
        #region Relacionamento com a Entidade Usuario

        /// <summary>
        /// Referencia para a entidade de dominio Usuario (ca_usuarios).
        /// Vinculado via Usuario.idUserAspNet.
        /// Usado APENAS para carregar dados completos do usuario durante o login.
        /// </summary>
        public virtual Usuario Usuario { get; set; }

        #endregion

        #region Construtores

        public CaUsuarioIdentity()
        {
        }

        /// <summary>
        /// Cria um CaUsuarioIdentity a partir de uma entidade Usuario existente.
        /// </summary>
        public CaUsuarioIdentity(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            Id = usuario.idUserAspNet;
            UserName = usuario.usuario;
            Email = usuario.email;
            PhoneNumber = usuario.tel1;
            LockoutEnabled = usuario.ativo == "N";
            Usuario = usuario;
        }

        #endregion
    }
}
