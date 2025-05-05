using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Usuario: Entity
    {
        public string nome { get; private set; }
        public string cpf { get; private set; }
        public string ender { get; private set; }
        public int? num { get; private set; }
        public string compl { get; private set; }
        public string bairro { get; private set; }
        public string cep { get; private set; }
        public string cidade { get; private set; }
        public string uf { get; private set; }
        public string tel1 { get; private set; }
        public string cel { get; private set; }
        public DateTime? dtnasc { get; private set; }
        public string usuario { get; private set; }
        public string senha { get; private set; }
        public string email { get; private set; }
        public string foto { get; private set; }
        public string tel2 { get; private set; }
        public string ativo { get; private set; }
        public DateTime? DataCadastro { get; private set; }
        public int? id_perfil { get; private set; }
        public string idUserAspNet { get; set; }
        public long? idPerfil { get; set; }

        public virtual List<EmpresaAuth> EmpresasAuth { get; set; } = new List<EmpresaAuth>();
        public virtual List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();
        public virtual List<Turno> TurnoInicial { get; set; } = new List<Turno>();
        public virtual List<Turno> TurnoFinal { get; set; } = new List<Turno>();
        public virtual List<CaixaMoeda> CaixaMoeda { get; private set; } = new List<CaixaMoeda>();
        public virtual List<Perda> Perdas { get; set; } = new List<Perda>();
        public virtual List<InventarioItem> InventarioItem { get; set; } = new List<InventarioItem>();
        public Usuario()
        {

        }

        public void Desativar()
        {
            var _ativo = (int)EAtivo.Inativo;
            ativo = _ativo.ToString();
        }

        public void Ativar()
        {
            var _ativo = (int)EAtivo.Ativo;
            ativo = _ativo.ToString();
        }

        public Usuario(string nome, string cpf, string ender, int? num, string compl, string bairro, string cep, string cidade, string uf, string tel1, string cel, DateTime? dtnasc, string usuario, string senha, string email, string foto, string tel2, string ativo, DateTime? dtcad, int? id_perfil, string idUserAspNet)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.ender = ender;
            this.num = num;
            this.compl = compl;
            this.bairro = bairro;
            this.cep = cep;
            this.cidade = cidade;
            this.uf = uf;
            this.tel1 = tel1;
            this.cel = cel;
            this.dtnasc = dtnasc;
            this.usuario = usuario;
            this.senha = senha;
            this.email = email;
            this.foto = foto;
            this.tel2 = tel2;
            this.ativo = ativo;
            this.DataCadastro = dtcad;
            this.id_perfil = id_perfil;
            this.idUserAspNet = idUserAspNet;
        }

        public Usuario(string nome, string ativo, string idUserAspNet)
        {
            this.nome = nome;
            this.ativo = ativo;
            this.idUserAspNet = idUserAspNet;
        }

        public Usuario(string nome, string cpf, string usuario, string email, string ativo, string idUserAspNet)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.usuario = usuario;
            this.email = email;
            this.ativo = ativo;
            this.idUserAspNet = idUserAspNet;
        }

        public Usuario(string foto, string idUserAspNet)
        {
            this.foto = foto;
            this.idUserAspNet = idUserAspNet;
        }

        public void AutalizarFoto(string _foto) => this.foto = _foto; 
    }
}
