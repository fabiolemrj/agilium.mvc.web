﻿using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.MoedaViewModel
{
    public class MoedaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "Tipo Situção Fiscal")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoDocFiscal? TipoDocFiscal { get; set; }
        [Display(Name = "Tipo Moeda")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoMoeda? Tipo { get; set; }
        [Display(Name = "% Taxa")]
        public double? PorcentTaxa { get; set; }
        [Display(Name = "Situação Troco")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESituacaoTroco? SitucacaoTroco { get; set; }
        [Display(Name = "Cor Botão")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string COR_BOTAO { get; set; }
        [Display(Name = "Cor Fonte")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string COR_FONTE { get; set; }
        [Display(Name = "Tecla Atalho")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string TECLA_ATALHO { get; set; }
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
    }
}
