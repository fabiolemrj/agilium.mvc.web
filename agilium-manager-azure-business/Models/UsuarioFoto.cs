using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class UsuarioFoto
    {
        public string Id { get; private set; }
        public string IdUsuarioAspNet { get; private set; }
        public string NomeUsuario { get; private set; }
        public string Descricao { get; private set; }
        public string Imagem { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Ativo { get; private set; }
        public string NomeArquivo { get; private set; }
        public long IdUsuario { get; private set; }
        public UsuarioFoto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UsuarioFoto(string idUsuarioAspNet, string nomeUsuario, string descricao, string imagem, DateTime dataCadastro, string ativo, string nomeArquivo, long idUsuario)
        {
            Id = Guid.NewGuid().ToString();
            IdUsuarioAspNet = idUsuarioAspNet;
            NomeUsuario = nomeUsuario;
            Descricao = descricao;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            Ativo = ativo;
            NomeArquivo = nomeArquivo;
            IdUsuario = idUsuario;
        }

        public UsuarioFoto(string id, string idUsuarioAspNet, string nomeUsuario, string descricao, string imagem, DateTime dataCadastro, string ativo, string nomeArquivo, long idUsuario)
        {
            Id = id;
            IdUsuarioAspNet = idUsuarioAspNet;
            NomeUsuario = nomeUsuario;
            Descricao = descricao;
            Imagem = imagem;
            DataCadastro = dataCadastro;
            Ativo = ativo;
            NomeArquivo = nomeArquivo;
            IdUsuario = idUsuario;
        }
    }
}
