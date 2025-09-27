

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace PassCrypto
{
    /// <summary>
    /// Exceção para erros de parâmetros.
    /// </summary>
    public class PassCryptoException : Exception
    {
        public PassCryptoException(string message) : base(message) { }
    }

    /// <summary>
    /// Tipos de método de criptografia/descriptografia.
    /// Usamos o atributo Flags para permitir a seleção de múltiplos métodos.
    /// </summary>
    [Flags]
    public enum MethodType
    {
        None = 0,
        Blowfish64 = 1,
        XChange = 2,
        DES64 = 4,
        HEX = 8,
        XOR = 16
    }

    /// <summary>
    /// Modo de operação: codificar ou decodificar.
    /// </summary>
    public enum FunctionMode
    {
        Encode,
        Decode
    }

    /// <summary>
    /// Implementa a lógica de criptografia e descriptografia do componente Delphi.
    /// </summary>
    public class PassCryptoService
    {
        // Constantes de mensagens de erro traduzidas
        private const string ERROR_PARAMS1 = "Senha inválida!";
        private const string ERROR_PARAMS2 = "Palavra-chave inválida!";
        private const string ERROR_PARAMS3 = "Alfabeto inválido!";
        private const string ERROR_PARAMS4 = "Alfabeto inválido! Existem caracteres repetidos!";
        private const string ERROR_PARAMS5 = "Nenhum método de codificação foi selecionado!";

        // Chave de criptografia padrão. NÃO ALTERAR!
        private const string PW_KEY = "AaGiliUMSisTem@sTEmOmelHoRpDvD0M3rC@d0";
        // Alfabeto padrão considerado. NÃO ALTERAR!
        private const string PW_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÁÀÂ" +
            "ÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÑáàâãäéèêëíìîïóòôõöúùûüñ0123456789.,!?;:+-/*()[]" +
            "{}%&$#@=_\"¤ºª°¹²³ØÆ§€£¥µ¶ÞþßÐð¢×øæ¬¦«»¡¼½¾®©¿<>±¸ýÿ ";

        private string _passWord;
        private string _keyWord;
        private string _alphabet;
        private FunctionMode _mode;
        private MethodType _method;

        /// <summary>
        /// Senha a ser criptografada/descriptografada.
        /// </summary>
        public string PassWord
        {
            get => _passWord;
            set => _passWord = value;
        }

        /// <summary>
        /// Chave de criptografia.
        /// </summary>
        public string KeyWord
        {
            get => _keyWord;
            set => _keyWord = value;
        }

        /// <summary>
        /// Alfabeto considerado para o método XChange.
        /// </summary>
        public string Alphabet
        {
            get => _alphabet;
            set => _alphabet = value;
        }

        /// <summary>
        /// Método de criptografia a ser usado.
        /// </summary>
        public MethodType Method
        {
            get => _method;
            set => _method = value;
        }

        /// <summary>
        /// Modo de operação (Codificar/Decodificar).
        /// </summary>
        public FunctionMode Mode
        {
            get => _mode;
            set => _mode = value;
        }

        public PassCryptoService()
        {
            // Inicializa as propriedades com valores padrão, como no Delphi
            _alphabet = PW_ALPHABET;
            _keyWord = PW_KEY;
            _method = MethodType.XChange;
            _mode = FunctionMode.Encode;
        }

        /// <summary>
        /// Método principal para criptografar ou descriptografar a senha.
        /// </summary>
        public string PasswrdCrypto()
        {
            DoCheckParams();

            switch (Mode)
            {
                case FunctionMode.Encode:
                    return DoEncode();
                case FunctionMode.Decode:
                    return DoDecode();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Realiza a criptografia seguindo a ordem de métodos.
        /// </summary>
        private string DoEncode()
        {
            var buffer = new StringBuilder(PassWord);

            if (Method.HasFlag(MethodType.XChange))
            {
                buffer = new StringBuilder(DoChange(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.XOR))
            {
                buffer = new StringBuilder(DoXOREncode(KeyWord, buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.Blowfish64))
            {
                buffer = new StringBuilder(DoBlowfish64Encode(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.DES64))
            {
                buffer = new StringBuilder(DoDES64Encode(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.HEX))
            {
                buffer = new StringBuilder(DoHEXEncode(buffer.ToString()));
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Realiza a descriptografia seguindo a ordem inversa.
        /// </summary>
        private string DoDecode()
        {
            var buffer = new StringBuilder(PassWord);

            if (Method.HasFlag(MethodType.HEX))
            {
                buffer = new StringBuilder(DoHEXDecode(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.DES64))
            {
                buffer = new StringBuilder(DoDES64Decode(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.Blowfish64))
            {
                buffer = new StringBuilder(DoBlowfish64Decode(buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.XOR))
            {
                buffer = new StringBuilder(DoXORDecode(KeyWord, buffer.ToString()));
            }

            if (Method.HasFlag(MethodType.XChange))
            {
                buffer = new StringBuilder(DoChange(buffer.ToString()));
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Valida os parâmetros de entrada.
        /// </summary>
        private void DoCheckParams()
        {
            var errorMessages = new StringBuilder();

            if (string.IsNullOrEmpty(PassWord))
            {
                errorMessages.AppendLine(ERROR_PARAMS1);
            }

            if (string.IsNullOrEmpty(KeyWord))
            {
                errorMessages.AppendLine(ERROR_PARAMS2);
            }

            if (Method == MethodType.None)
            {
                errorMessages.AppendLine(ERROR_PARAMS5);
            }

            if (Method.HasFlag(MethodType.XChange))
            {
                if (string.IsNullOrEmpty(Alphabet))
                {
                    errorMessages.AppendLine(ERROR_PARAMS3);
                }
                else if (Alphabet.Distinct().Count() != Alphabet.Length)
                {
                    errorMessages.AppendLine(ERROR_PARAMS4);
                }
            }

            if (errorMessages.Length > 0)
            {
                errorMessages.AppendLine().AppendLine("Contate o administrador.");
                throw new PassCryptoException(errorMessages.ToString());
            }
        }

        /// <summary>
        /// Realiza a codificação/descodificação usando a substituição de caracteres.
        /// </summary>
        private string DoChange(string aText)
        {
            if (Mode == FunctionMode.Encode)
            {
                return DoChangeEncode(KeyWord, aText);
            }
            else
            {
                return DoChangeDecode(KeyWord, aText);
            }
        }

        /// <summary>
        /// Codifica a string usando o método de substituição.
        /// </summary>
        private string DoChangeEncode(string aKey, string aPassWord)
        {
            const int CONSTANTE = 64;
            var result = new StringBuilder(aPassWord);
            int keyIndex = 0;

            for (int i = 0; i < result.Length; i++)
            {
                char currentChar = result[i];
                int alphabetIndex = Alphabet.IndexOf(currentChar);

                if (alphabetIndex >= 0)
                {
                    int keyCharValue = (int)aKey[keyIndex] - CONSTANTE + 1;
                    int newIndex = alphabetIndex + keyCharValue;

                    if (newIndex >= Alphabet.Length)
                    {
                        newIndex %= Alphabet.Length;
                    }

                    if (newIndex < 0)
                    {
                        newIndex = (newIndex % Alphabet.Length + Alphabet.Length) % Alphabet.Length;
                    }

                    result[i] = Alphabet[newIndex];
                }

                keyIndex++;
                if (keyIndex >= aKey.Length)
                {
                    keyIndex = 0;
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Descodifica a string usando o método de substituição.
        /// </summary>
        private string DoChangeDecode(string aKey, string aPassWord)
        {
            const int CONSTANTE = 64;
            var result = new StringBuilder(aPassWord);
            int keyIndex = 0;

            for (int i = 0; i < result.Length; i++)
            {
                char currentChar = result[i];
                int alphabetIndex = Alphabet.IndexOf(currentChar);

                if (alphabetIndex >= 0)
                {
                    int keyCharValue = (int)aKey[keyIndex] - CONSTANTE + 1;
                    int newIndex = alphabetIndex - keyCharValue;

                    if (newIndex < 0)
                    {
                        newIndex = (newIndex % Alphabet.Length + Alphabet.Length) % Alphabet.Length;
                    }
                    else if (newIndex >= Alphabet.Length)
                    {
                        newIndex %= Alphabet.Length;
                    }

                    result[i] = Alphabet[newIndex];
                }

                keyIndex++;
                if (keyIndex >= aKey.Length)
                {
                    keyIndex = 0;
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Codifica a senha para seu valor hexadecimal.
        /// </summary>
        private string DoHEXEncode(string aPassWord)
        {
            var sb = new StringBuilder();
            foreach (var c in aPassWord)
            {
                sb.Append(((int)c).ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Decodifica a senha de seu valor hexadecimal.
        /// </summary>
        private string DoHEXDecode(string aPassWord)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < aPassWord.Length; i += 2)
            {
                string hex = aPassWord.Substring(i, 2);
                sb.Append((char)Convert.ToInt32(hex, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Realiza a criptografia usando o operador XOR.
        /// </summary>
        private string DoXOREncode(string aKey, string aPassWord)
        {
            var result = new StringBuilder(aPassWord);
            var keyBytes = Encoding.UTF8.GetBytes(aKey);

            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < keyBytes.Length; j++)
                {
                    result[i] = (char)(result[i] ^ keyBytes[j]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Realiza a descriptografia usando o operador XOR.
        /// </summary>
        private string DoXORDecode(string aKey, string aPassWord)
        {
            var result = new StringBuilder(aPassWord);
            var keyBytes = Encoding.UTF8.GetBytes(aKey);

            for (int i = 0; i < result.Length; i++)
            {
                for (int j = keyBytes.Length - 1; j >= 0; j--)
                {
                    result[i] = (char)(result[i] ^ keyBytes[j]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Codifica a string usando o método Blowfish de 64 bits.
        /// Este método agora usa a biblioteca Bouncy Castle.
        /// </summary>
        private string DoBlowfish64Encode(string aPassWord)
        {
            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(KeyWord);
                var engine = new BlowfishEngine();
                var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());
                cipher.Init(true, new KeyParameter(keyBytes));

                var inputBytes = Encoding.UTF8.GetBytes(aPassWord);
                var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
                var length = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
                length += cipher.DoFinal(outputBytes, length);

                return Convert.ToBase64String(outputBytes, 0, length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na criptografia Blowfish: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Decodifica a string usando o método Blowfish de 64 bits.
        /// Este método agora usa a biblioteca Bouncy Castle.
        /// </summary>
        private string DoBlowfish64Decode(string aPassWord)
        {
            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(KeyWord);
                var engine = new BlowfishEngine();
                var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());
                cipher.Init(false, new KeyParameter(keyBytes));

                var inputBytes = Convert.FromBase64String(aPassWord);
                var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
                var length = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
                length += cipher.DoFinal(outputBytes, length);

                return Encoding.UTF8.GetString(outputBytes, 0, length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na descriptografia Blowfish: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Codifica a string usando o método DES de 64 bits.
        /// </summary>
        private string DoDES64Encode(string aPassWord)
        {
            try
            {
                using (var des = DES.Create())
                {
                    // Chave e IV precisam ser do tamanho correto
                    des.Key = DeriveKey(KeyWord, des.KeySize / 8);
                    des.IV = DeriveKey(KeyWord, des.BlockSize / 8);

                    using (var encryptor = des.CreateEncryptor(des.Key, des.IV))
                    {
                        var inputBytes = Encoding.UTF8.GetBytes(aPassWord);
                        var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na criptografia DES: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Decodifica a string usando o método DES de 64 bits.
        /// </summary>
        private string DoDES64Decode(string aPassWord)
        {
            try
            {
                using (var des = DES.Create())
                {
                    // Chave e IV precisam ser do tamanho correto
                    des.Key = DeriveKey(KeyWord, des.KeySize / 8);
                    des.IV = DeriveKey(KeyWord, des.BlockSize / 8);

                    using (var decryptor = des.CreateDecryptor(des.Key, des.IV))
                    {
                        var inputBytes = Convert.FromBase64String(aPassWord);
                        var decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na descriptografia DES: {ex.Message}");
                return string.Empty;
            }
        }

        // Método auxiliar para derivar uma chave do KeyWord para DES.
        private byte[] DeriveKey(string password, int keySize)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var key = new byte[keySize];
                Array.Copy(hash, key, keySize);
                return key;
            }
        }
    }
}
