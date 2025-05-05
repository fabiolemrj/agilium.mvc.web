using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Unidade: Entity
    {
        public string Codigo { get; private set; }
        public string Descricao { get; private set; }
        public EAtivo? Ativo { get; private set; } = EAtivo.Ativo;
        
        public Unidade()
        {

        }

        public Unidade(string codigo, string descricao, EAtivo? ativo)
        {
            Codigo = codigo;
            Descricao = descricao;
            Ativo = ativo;
        }

        public void Ativar() => Ativo = EAtivo.Ativo;
        public void Desativar() => Ativo = EAtivo.Inativo;
    }
}
