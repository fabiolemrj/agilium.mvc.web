using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    /// <summary>
    /// Representa as telas e recursos que o usuario terá acesso no sistema
    /// </summary>
    public class CaPermissaoItem : Entity
    {
        public string Descricao { get; private set; }
        public string Situacao { get; private set; }
        public virtual List<CaModelo> Modelos { get; private set; }
        public CaPermissaoItem()
        {
            Modelos = new List<CaModelo>();
        }
        public CaPermissaoItem(string descricao, string situacao)
        {
            Modelos = new List<CaModelo>();

            Descricao = descricao;
            Situacao = situacao;
        }

        public void MudarSituacao(string situacao) => Situacao = situacao;
        public void AdicionarModelo(CaModelo caModelo) => Modelos.Add(caModelo);
        public void RemoverModelo(CaModelo caModelo) => Modelos.Remove(caModelo);
    }
}
