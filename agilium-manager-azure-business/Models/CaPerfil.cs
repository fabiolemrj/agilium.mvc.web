using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CaPerfil : Entity
    {
        public long idEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
        public string Descricao { get; private set; }
        public string Situacao { get; private set; }

        public virtual List<CaModelo> Modelos { get; private set; }
        public CaPerfil()
        {
            Modelos = new List<CaModelo>();
        }

        public CaPerfil(string descricao, string situacao)
        {
            Modelos = new List<CaModelo>();

            Descricao = descricao;
            Situacao = situacao;
        }

        public void AlterarSituacao(string situacao) => Situacao = situacao;
        public void AdicionarModelo(CaModelo caModelo) => Modelos.Add(caModelo);
        public void RemoverModelo(CaModelo caModelo) => Modelos.Remove(caModelo);
    }
}
