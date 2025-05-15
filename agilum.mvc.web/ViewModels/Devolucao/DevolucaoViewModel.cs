using agilium.api.business.Enums;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using agilum.mvc.web.ViewModels.Empresa;

namespace agilum.mvc.web.ViewModels.Devolucao
{
    public class DevolucaoViewModel
    {
        public long Id { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public Int64? IDCLIENTE { get; set; }
        public string ClienteNome { get; set; }
        public Int64? IDMOTDEV { get; set; }
        public string MotivoDevolucaoNome { get; set; }
        public Int64? IDVALE { get; set; }
        public string ValeNome { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        public double? ValorTotal { get; set; }
        public string Observacao { get; set; }
        public ESituacaoDevolucao? Situacao { get; set; }
        public string CaixaNome { get; set; }
        public string VendaData { get; set; }
        public List<DevolucaoItemViewModel> Itens { get; set; } = new List<DevolucaoItemViewModel>();
        public List<DevolucaoItemVendaViewModel> DevolucaoItens { get; set; } = new List<DevolucaoItemVendaViewModel>();

    }

    public class DevolucaoItemViewModel
    {
        public long Id { get; set; }
        public string DevolucaoNome { get; set; }
        public Int64? IDDEV { get; set; }
        public string VendaItemNome { get; set; }
        public Int64? IDVENDA_ITEM { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorItem { get; set; }
        public string ProdutoNome { get; set; }
        public int? SequencialVenda { get; set; }
        public double? ValorItemVenda { get; set; }
        public string CodigoProduto { get; set; }
    }

    public class DevolucaoItemVendaViewModel
    {
        public long idDevolucao { get; set; }
        public bool selecionado { get; set; }
        public int SeqVenda { get; set; }
        public string ProdutoNome { get; set; }
        public double QuantidadeVendida { get; set; }
        public double ValorVendido { get; set; }
        public double QuantidadeDevolucao { get; set; }
        public double ValorDevolucao { get; set; }
        public long idItemVenda { get; set; }
        public long idDevolucaoItem { get; set; }
        public long idProduto { get; set; }
        public double ValorTotal { get; set; }
    }

    public class MotivoDevolucaoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public long? idEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(30, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }
}
