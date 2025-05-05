using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace agilium.api.manager.Services
{
    public static class Utils
    {
        public static Byte[] ObterImagemFile(string arquivo)
        {
            var path =  MontarCaminhoArquivoFotoUsuario(arquivo);

            byte[] binaryImage = System.IO.File.ReadAllBytes(path);
            return binaryImage;
        }

        public static string MontarCaminhoArquivoFotoUsuario(string arquivo)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Foto", arquivo);
        }

        public static IFormFile ConverterToIFormFile(byte[] file, string fileName, string fileNameExtensao)
        {
            var stream = new MemoryStream(file);
            return new FormFile(stream, 0, stream.Length, fileName, fileNameExtensao);
        }

        public static byte[] ConverterToIFormFile(string fileBase64)
        {
            return Convert.FromBase64String(fileBase64);
        }

        public static string ConverterByteToString(byte[] byteArray)
        {
            return Encoding.Default.GetString(byteArray);
        }

        public static string ConverterByteToBase64(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }
    }
}
