using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CaModelo : Entity
    {
        public virtual CaPerfil CaPerfil { get; set; }
        public long idPerfil { get; private set; }
        public virtual CaPermissaoItem CaPermissaoItem { get; set; }
        public long idPermissao { get; private set; }
        public string Incluir { get; private set; }
        public string Alterar { get; private set; }
        public string Excluir { get; private set; }
        public string Relatorio { get; private set; }
        public string Consulta { get; private set; }
        public CaModelo()
        {

        }

        public CaModelo(CaPerfil perfil, CaPermissaoItem permissaoItem)
        {
            CaPerfil = perfil;
            CaPermissaoItem = permissaoItem;
        }

        public CaModelo(long idPerfil, long idPermissao)
        {
            this.idPerfil = idPerfil;
            this.idPermissao = idPermissao;
        }

        public CaModelo(string incluir, string alterar, string excluir, string relatorio, string consulta)
        {
            Incluir = incluir;
            Alterar = alterar;
            Excluir = excluir;
            Relatorio = relatorio;
            Consulta = consulta;
        }

        public CaModelo(long idPerfil, long idPermissao, string incluir, string alterar, string excluir, string relatorio, string consulta) : this(idPerfil, idPermissao)
        {
            Incluir = incluir;
            Alterar = alterar;
            Excluir = excluir;
            Relatorio = relatorio;
            Consulta = consulta;
        }

        public CaModelo(CaPerfil caPerfil, CaPermissaoItem caPermissaoItem, string incluir, string alterar, string excluir, string relatorio, string consulta) : this(caPerfil, caPermissaoItem)
        {
            Incluir = incluir;
            Alterar = alterar;
            Excluir = excluir;
            Relatorio = relatorio;
            Consulta = consulta;
        }

        public void AdicionarPermissao(string incluir, string alterar, string excluir, string relatorio, string consulta)
        {
            Incluir = incluir;
            Alterar = alterar;
            Excluir = excluir;
            Relatorio = relatorio;
            Consulta = consulta;
        }
        public void AdicionarPermissaoInclusao(string permissao) => Incluir = permissao;
        public void AdicionarPermissaoAlteracao(string permissao) => Alterar = permissao;
        public void AdicionarPermissaoExclusao(string permissao) => Excluir = permissao;
        public void AdicionarPermissaoRelatorio(string permissao) => Relatorio = permissao;
        public void AdicionarPermissaoConsulta(string permissao) => Consulta = permissao;

        public void AddIdPerfil(long idperfil) => idPerfil = idperfil;
    }
}
