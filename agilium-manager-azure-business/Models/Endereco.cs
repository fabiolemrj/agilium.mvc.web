using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Endereco: Entity
    {
        public string Logradouro { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cep { get; private set; }
        public string Cidade { get; private set; }
        public string Uf { get; private set; }
        public string Pais { get; private set; }
        public int? Ibge { get; private set; }
        public string PontoReferencia { get; private set; }
        public string Numero { get; private set; }

        public virtual List<Empresa> Empresas { get; set; } = new List<Empresa>();
        public virtual List<Fornecedor> Fornecedores { get; set; } = new List<Fornecedor>();

        public virtual List<Cliente> ClienteEndereco { get; set; } = new List<Cliente>();
        public virtual List<Cliente> ClienteEnderecoCobranca { get; set; } = new List<Cliente>();
        public virtual List<Cliente> ClienteEnderecoFaturamento { get; set; } = new List<Cliente>();
        public virtual List<Cliente> ClienteEnderecoEntrega { get; set; } = new List<Cliente>();
        public virtual List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public Endereco()
        {

        }

        public Endereco(string logradouro, string complemento, string bairro, string cep, string cidade, string uf, string pais, int? ibge, string pontoReferencia, string numero)
        {
            Logradouro = logradouro;
            Complemento = complemento;
            Bairro = bairro;
            Cep = cep;
            Cidade = cidade;
            Uf = uf;
            Pais = pais;
            Ibge = ibge;
            PontoReferencia = pontoReferencia;
            Numero = numero;
        }
    }
}
