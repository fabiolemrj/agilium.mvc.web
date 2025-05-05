using agilium.webapp.manager.mvc.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.ViewModels.Config
{
    public class ConfigCamposViewModel
    {
        public long IdEmpresa { get; set; }

        [Display(Name = "Cor Final")]
        public ChaveValorViewModel COR_FINAL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Cor Fundo Titulo")]
        public ChaveValorViewModel COR_FONTE_TIT_EXCEL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Cor da Fonte")]
        public ChaveValorViewModel COR_FONTE_ZEBRADA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Zebrada Excel")]
        public ChaveValorViewModel COR_FONTE_ZEBRADA_EXCEL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Fundo Fonte do Titulo")]
        public ChaveValorViewModel COR_FUNDO_TIT_EXCEL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Cor inicial")]
        public ChaveValorViewModel COR_INICIAL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Cor Tema")]
        public ChaveValorViewModel COR_TEMA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Cor Zebrada")]
        public ChaveValorViewModel COR_ZEBRADA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Pasta Fotos")]
        public ChaveValorViewModel PASTA_FOTOS { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Porta Impressora")]
        public ChaveValorViewModel PORTA_IMPRESSORA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Estoque Permite Negativo")]
        public ChaveValorViewModel ESTOQUE_PERMITENEGATIVO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Caminho")]
        public ChaveValorViewModel CERTIFICADO_CAMINHO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Senha")]
        public ChaveValorViewModel CERTIFICADO_SENHA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Descrição Padrão Suprimento")]
        public ChaveValorViewModel CAIXA_DSSUPRIMENTO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Valor Padrão Suprimento (R$)")]
        public ChaveValorViewModel CAIXA_VLSUPRIMENTO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Ao abrir um novo Caixa")]
        public ChaveValorViewModel CAIXA_TPABERTURA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Modelo NFC-e")]
        public ChaveValorViewModel NFCE_MODELO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Serie NFC-e")]
        public ChaveValorViewModel NFCE_SERIE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Texto da Natureza da Operação da Venda")]
        public ChaveValorViewModel NFCE_NATOP { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Numero da Ultima NFC-e emitida")]
        public ChaveValorViewModel NFCE_ULTEMITIDA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Modelo NFC-e")]
        public ChaveValorViewModel NFCE_MODELO_HOMOL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Serie NFC-e")]
        public ChaveValorViewModel NFCE_SERIE_HOMOL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Numero Ultima NFC-e emitida")]
        public ChaveValorViewModel NFCE_NATOP_HOMOL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Numero da Ultima NFC-e emitida")]
        public ChaveValorViewModel NFCE_ULTEMITIDA_HOMOL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Ambiente Atual")]
        public ChaveValorViewModel NFCE_AMBIENTE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Conta Registro Estoque")]
        public ChaveValorViewModel CONTA_IDCONTAESTOQUE { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel CONTA_NMCONTAESTOQUE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Conta Registro Fornecedor")]
        public ChaveValorViewModel CONTA_IDCONTAFORNECEDOR { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel CONTA_NMCONTAFORNECEDOR { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Conta Registro Cliente")]
        public ChaveValorViewModel CONTA_IDCONTACLIENTE { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel CONTA_NMCONTACLIENTE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Realizar controle lançamentos contabeis")]
        public ChaveValorViewModel CONTA_REALIZARCONTROLE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "SMTP")]
        public ChaveValorViewModel MAIL_SMTP { get; set; } = new ChaveValorViewModel();
        [Display(Name = "POP")]
        public ChaveValorViewModel MAIL_POP { get; set; } = new ChaveValorViewModel();
        [Display(Name = "E-mail")]
        public ChaveValorViewModel MAIL_EMAIL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Usuario")]
        public ChaveValorViewModel MAIL_USUARIO { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Senha")]
        public ChaveValorViewModel MAIL_SENHA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Remetente")]
        public ChaveValorViewModel MAIL_REMETENTE { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Requer autenticação")]
        public ChaveValorViewModel MAIL_AUTENTICA { get; set; } = new ChaveValorViewModel();
        [Display(Name = "conexão segurança (SSL)")]
        public ChaveValorViewModel MAIL_SSL { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Porta Saida (SMTP)")]
        public ChaveValorViewModel MAIL_PORTA_SMTP { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Porta Entrada (POP)")]
        public ChaveValorViewModel MAIL_PORTA_POP { get; set; } = new ChaveValorViewModel();
        [Display(Name = "Conta Registro Estoque")]
        public ChaveValorViewModel VENDAS_DOC_FISCAL_PADRAO_STR { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel VENDAS_DOC_FISCAL_PADRAO { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel PDV_PREVENDA { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel PDV_TAMANHO_FONTE { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel TXENTREGA_VLMINIMO { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel TXENTREGA_COBRAR { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel TXENTREGA_FORMA { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel TXENTREGA_VALOR { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel PDV_IMPRESSORA_COZINHA { get; set; } = new ChaveValorViewModel();
        public ChaveValorViewModel PDV_PORTA_IMPRESSORA_COZINHA { get; set; } = new ChaveValorViewModel();
    }

    public class ChaveValorViewModel
    {
        public long IDEMPRESA { get; set; }
        public string CHAVE { get; set; }
        public string VALOR { get; set; }
        public IFormFile Arquivo { get; set; }
    }

    public class EditarChaveValorViewModel
    {
        public long IdEmpresa { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public ETipoCompnenteConfig Tipo { get; set; }
        public string Label { get; set; }
        public EClassificacaoConfiguracao Classificacao { get; set; }
    }


}
