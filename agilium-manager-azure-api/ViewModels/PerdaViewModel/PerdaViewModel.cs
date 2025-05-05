using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.PerdaViewModel
{
    public class PerdaViewModel
    {
        public long Id { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        public Int64? IDESTOQUE { get; set; }
        public string EstoqueNome { get; set; }
        public Int64? IDESTOQUEHST { get; set; }
        public string EstoqueHistoricoNome { get; set; }
        public string ProdutoNome { get; set; }
        public Int64? IDPRODUTO { get; set; }
        public Int64? IDUSUARIO { get; set; }
        public string UsuarioNome { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        public ETipoPerda? Tipo { get; set; }
        public ETipoMovimentoPerda? Movimento { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorCustoMedio { get; set; }
        public string Observacao { get; set; }
    }
}
