using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class UsuarioFotoEntity:Entity
    {
        public string IdUsuarioAspNet { get; private set; }
        public string NomeArquivo { get; private set; }
        public byte[] Imagem { get; private set; }
        public DateTime DataCadastro { get; private set; }    
        public long IdUsuario { get; private set; }

        public UsuarioFotoEntity()
        {

        }
     
        public void AdicionarImagem(byte[] imagem)
        {
            Imagem = imagem;
        }

        public UsuarioFotoEntity(string idUsuarioAspNet, string nomeArquivo, byte[] imagem, DateTime dataCadastro, long idUsuario)
        {
            IdUsuarioAspNet = idUsuarioAspNet;
            NomeArquivo = nomeArquivo;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            IdUsuario = idUsuario;
        }

        public UsuarioFotoEntity(long id, string idUsuarioAspNet, string nomeArquivo, byte[] imagem, DateTime dataCadastro, long idUsuario)
        {
            Id = id;
            IdUsuarioAspNet = idUsuarioAspNet;
            NomeArquivo = nomeArquivo;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            IdUsuario = idUsuario;
        }


    }
}
