using System.Collections.Generic;
using System;
using agilium.webapp.manager.mvc.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using agilium.webapp.manager.mvc.ViewModels.Cliente;
using agilium.webapp.manager.mvc.ViewModels.Empresa;

namespace agilium.webapp.manager.mvc.ViewModels.DevolucaoViewModel
{
    public class DevolucaoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        [Display(Name = "Venda")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        [Display(Name = "Cliente")]
        public Int64? IDCLIENTE { get; set; }
        public string ClienteNome { get; set; }
        [Display(Name = "Motivo Devolução")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDMOTDEV { get; set; }
        public string MotivoDevolucaoNome { get; set; }
        [Display(Name = "Vale")]
        public Int64? IDVALE { get; set; }
        public string ValeNome { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        [Display(Name = "Total da Devolução")]
        public double? ValorTotal { get; set; }
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        public string CaixaNome { get; set; }
        public string VendaData { get; set; }
        public ESituacaoDevolucao? Situacao { get; set; }
        public List<DevolucaoItemViewModel> Itens { get; set; } = new List<DevolucaoItemViewModel>();
        public List<VendaViewModel.VendaViewModel> Vendas { get; set; } = new List<VendaViewModel.VendaViewModel>();
        [Display(Name = "Listar vendas da data")]
        public DateTime DataConsulta { get; set; }
        public List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<MotivoDevolucaoViewModel> MotivosDevolucao { get; set; } = new List<MotivoDevolucaoViewModel>();
        public List<DevolucaoItemVendaViewModel> DevolucaoItens { get; set; } = new List<DevolucaoItemVendaViewModel>();
    }

    public class DevolucaoItemViewModel
    {
        public long Id { get; set; }
        public string DevolucaoNome { get; set; }
        [Display(Name = "Devolução")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
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

    public class DevolucaoEditarViewModel
    {
        public long Idvenda { get; set; }
        public List<DevolucaoItemEditarViewModel> VendasItens { get; set; } = new List<DevolucaoItemEditarViewModel>();
    }

    public class DevolucaoItemEditarViewModel
    {
        public long idVenda { get; set; }
        public string VendaNome { get; set; }
    }

    public class DevolucaoItemVendaViewModel
    {
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
        public long idDevolucao { get; set; }
        public DevolucaoItemVendaViewModel()
        {            
        }

        public DevolucaoItemVendaViewModel(bool selecionado, int seqVenda, string produtoNome, double quantidadeVendida, double valorVendido, double quantidadeDevolucao, double valorDevolucao, long idItemVenda, long idDevolucaoItem, long idProduto, double valorTotal, long idDevolucao)
        {
            this.selecionado = selecionado;
            SeqVenda = seqVenda;
            ProdutoNome = produtoNome;
            QuantidadeVendida = quantidadeVendida;
            ValorVendido = valorVendido;
            QuantidadeDevolucao = quantidadeDevolucao;
            ValorDevolucao = valorDevolucao;
            this.idItemVenda = idItemVenda;
            this.idDevolucaoItem = idDevolucaoItem;
            this.idProduto = idProduto;
            ValorTotal = valorTotal;
            this.idDevolucao = idDevolucao;
        }
    }

}
