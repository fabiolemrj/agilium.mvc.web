using agilium.api.business.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace agilium.api.business.Services
{
    /// <summary>
    /// PasswordHasher customizado que suporta:
    /// 1. Validacao de senhas MD5 legadas (RetornaMD5 do Pascal - ca_usuarios.senha original)
    /// 2. Re-hash automatico para PBKDF2 (ASP.NET Identity padrao) no primeiro login bem-sucedido
    /// 3. Compatibilidade total com IdentityUser (padrao PBKDF2)
    ///
    /// O sistema legado em Pascal usa a funcao RetornaMD5(senha) que gera hash MD5
    /// em hexadecimal minusculo (32 caracteres), conforme implementacao MD5String da unit md5.pas.
    /// Exemplo: MD5String('admin') → '21232f297a57a5a743894a0e4a801fc3'
    /// </summary>
    public class CaUsuarioPasswordHasher : PasswordHasher<CaUsuarioIdentity>
    {
        /// <summary>
        /// Hash da senha usando PBKDF2 (padrao ASP.NET Core Identity).
        /// </summary>
        public override string HashPassword(CaUsuarioIdentity user, string password)
        {
            return base.HashPassword(user, password);
        }

        /// <summary>
        /// Verifica a senha:
        /// 1. Tenta PBKDF2 padrao (Identity atual) primeiro
        /// 2. Se falhar, tenta MD5 legado (igual ao RetornaMD5 do Pascal)
        /// 3. Se MD5 for valido, retorna SuccessRehashNeeded para migrar para PBKDF2
        /// </summary>
        public override PasswordVerificationResult VerifyHashedPassword(CaUsuarioIdentity user, string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
                return PasswordVerificationResult.Failed;

            // 1. Tenta verificar como PBKDF2 (padrao ASP.NET Identity)
            var result = base.VerifyHashedPassword(user, hashedPassword, providedPassword);

            if (result != PasswordVerificationResult.Failed)
                return result;

            // 2. Se falhou o PBKDF2, tenta como MD5 legado (RetornaMD5 do Pascal)
            if (IsMd5Hash(hashedPassword))
            {
                var md5Hash = ComputeMd5Hash(providedPassword);

                if (string.Equals(hashedPassword, md5Hash, StringComparison.OrdinalIgnoreCase))
                {
                    // Senha MD5 valida! Retorna que precisa re-hash para PBKDF2
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }
            }

            // 3. Tenta MD5 com lowercase forcado (o Pascal pode ter armazenado lowercase)
            if (IsMd5Hash(hashedPassword))
            {
                var md5HashLower = ComputeMd5Hash(providedPassword).ToLowerInvariant();

                if (string.Equals(hashedPassword, md5HashLower, StringComparison.Ordinal))
                {
                    return PasswordVerificationResult.SuccessRehashNeeded;
                }
            }

            return PasswordVerificationResult.Failed;
        }

        #region Metodos Auxiliares MD5

        /// <summary>
        /// Verifica se o hash parece ser MD5 (32 caracteres hexadecimais).
        /// O MD5 do Pascal (MD5Print) produz 32 caracteres hex minusculos.
        /// </summary>
        private bool IsMd5Hash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 32)
                return false;

            foreach (char c in hash)
            {
                if (!Uri.IsHexDigit(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Calcula o hash MD5 de uma string.
        /// Equivalente ao MD5String do Pascal (unit md5.pas).
        /// Retorna hexadecimal minusculo (32 caracteres), igual ao MD5Print.
        /// </summary>
        private string ComputeMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        #endregion
    }
}
