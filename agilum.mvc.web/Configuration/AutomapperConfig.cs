using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn;
using agilum.mvc.web.ViewModels.Caixa;
using agilum.mvc.web.ViewModels.CategeoriaFinanceira;
using agilum.mvc.web.ViewModels.Cliente;
using agilum.mvc.web.ViewModels.Compra;
using agilum.mvc.web.ViewModels.Conta;
using agilum.mvc.web.ViewModels.Contato;
using agilum.mvc.web.ViewModels.Devolucao;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.EmpresaUsuario;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.FormaPagamento;
using agilum.mvc.web.ViewModels.Fornecedor;
using agilum.mvc.web.ViewModels.Funcionarios;
using agilum.mvc.web.ViewModels.Impostos;
using agilum.mvc.web.ViewModels.Inventario;
using agilum.mvc.web.ViewModels.Moedas;
using agilum.mvc.web.ViewModels.Perda;
using agilum.mvc.web.ViewModels.PlanoConta;
using agilum.mvc.web.ViewModels.PontoVenda;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.Turno;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using agilum.mvc.web.ViewModels.Usuarios;
using agilum.mvc.web.ViewModels.Vale;
using agilum.mvc.web.ViewModels.Venda;
using agilum.mvc.web.ViewModels.Config;
using agilum.mvc.web.ViewModels.Log;
using agilum.mvc.web.ViewModels.Usuarios;

using AutoMapper;
using System.Collections.Generic;
using Endereco = agilium.api.business.Models.Endereco;
using agilum.mvc.web.ViewModels.Licenca;
using agilium_manager_azure_business.Models.CustomReturn.CompraViewModel;

namespace agilum.mvc.web.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region NFe
            CreateMap<agilium.api.business.Models.CustomReturn.ComprasNFEViewModel.NFeProc, agilum.mvc.web.ViewModels.Compra.NFeProc>()
                 .ForMember(dest => dest.NotaFiscalEletronica, opt => opt.MapFrom(src => src.NotaFiscalEletronica))
                 .ForMember(dest => dest.ArquivoXml, act => act.Ignore())
                 .ForMember(dest => dest.sucesso, act => act.Ignore())
                .ReverseMap();

            #endregion
            
            #region Controle Acessso / Usuarios
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
            CreateMap<agilium.api.business.Models.PagedResult<Usuario>, agilium.api.business.Models.PagedResult<UsuarioViewModel>>().ReverseMap();

            CreateMap<Usuario, UsuarioPadrao>()
                     .ForMember(dest => dest.dtnasc, opt => opt.MapFrom(src => src.DataCadastro.ToString()))
                     .ForMember(dest => dest.idperfilManager, opt => opt.MapFrom(src => src.id_perfil))
                      .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo)).ReverseMap();
            //.ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"));
            CreateMap<agilium.api.business.Models.PagedResult<Usuario>, agilium.api.business.Models.PagedResult<UsuarioPadrao>>().ReverseMap()
              .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List));

            CreateMap<Usuario, ListaUsuarioViewModel>()
                .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.nome))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CpfUsuario, opt => opt.MapFrom(x => x.cpf));


            CreateMap<EmpresaAuth, EmpresaUsuarioViewModel>()
                  // .ForMember(dest => dest.NomeEmpresa, opt => opt.MapFrom(src => src.Empresa.NMRZSOCIAL))
                  .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA.ToString()))
                  .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO.ToString()))
                .ReverseMap();

            CreateMap<Usuario, UserFull>()
               .ForMember(dest => dest.dtnasc, opt => opt.MapFrom(src => src.DataCadastro.ToString()))
               .ForMember(dest => dest.idperfilManager, opt => opt.MapFrom(src => src.id_perfil))
               .ForMember(dest => dest.UsuarioPossuiAcessoWeb, act => act.Ignore())
                .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo)).ReverseMap();

            CreateMap<CaPerfiManager, CaPerfilManagerViewModel>().ReverseMap();

            #endregion

            #region Unidade
            CreateMap<Unidade, UnidadeIndexViewModel>().ReverseMap();
            #endregion

            #region Empresa

            // Mapeamento simples Empresa → EmpresaViewModel
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();

            // Mapeamento usado no Identity
            CreateMap<Empresa, agilum.mvc.web.Areas.Identity.Pages.Account.LoginModel.EmpresaViewModel>()
                .ReverseMap();

            // Mapeamento principal Empresa → EmpresaCreateViewModel
            CreateMap<Empresa, EmpresaCreateViewModel>()
                .ForMember(dest => dest.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                // OBS: Model tem IDLOJA_SITEMARCADO, banco tem IDLOJA_SITEMERCADO
                .ForMember(dest => dest.IDLOJA_SITEMARCADO, opt => opt.MapFrom(src => src.IDLOJA_SITEMARCADO))
                .ReverseMap()

                // ❗ Nunca mapear listas na volta → isso destrói relacionamentos
                .ForMember(dest => dest.ContatoEmpresas, opt => opt.Ignore())
                .ForMember(dest => dest.Configuracoes, opt => opt.Ignore())
                .ForMember(dest => dest.EmpresasAuth, opt => opt.Ignore())
                .ForMember(dest => dest.ConfigImagem, opt => opt.Ignore())
                .ForMember(dest => dest.Perfil, opt => opt.Ignore())
                .ForMember(dest => dest.Estoques, opt => opt.Ignore())
                .ForMember(dest => dest.Funcionarios, opt => opt.Ignore())
                .ForMember(dest => dest.Moedas, opt => opt.Ignore())
                .ForMember(dest => dest.PontosVendas, opt => opt.Ignore())
                .ForMember(dest => dest.Produtos, opt => opt.Ignore())
                .ForMember(dest => dest.PlanoContas, opt => opt.Ignore())
                .ForMember(dest => dest.ContaPagar, opt => opt.Ignore())
                .ForMember(dest => dest.ContaReceber, opt => opt.Ignore())
                .ForMember(dest => dest.NotaFiscalInutil, opt => opt.Ignore())
                .ForMember(dest => dest.Turnos, opt => opt.Ignore())
                .ForMember(dest => dest.Caixas, opt => opt.Ignore())
                .ForMember(dest => dest.Vales, opt => opt.Ignore())
                .ForMember(dest => dest.Perdas, opt => opt.Ignore())
                .ForMember(dest => dest.Devolucao, opt => opt.Ignore())
                .ForMember(dest => dest.Compras, opt => opt.Ignore())
                .ForMember(dest => dest.Inventarios, opt => opt.Ignore())
                .ForMember(dest => dest.ProdutoSiteMercado, opt => opt.Ignore())
                .ForMember(dest => dest.MoedasSiteMercados, opt => opt.Ignore())

                // ❗ FK NÃO pode ser alterada via AutoMapper durante edição!
                .ForMember(dest => dest.IDENDERECO, opt => opt.Ignore())

                // ❗ Endereço é atualizado manualmente no Controller (como você já fez)
                .ForMember(dest => dest.Endereco, opt => opt.Ignore());

            #endregion


            #region Contato
            CreateMap<Contato, ContatoIndexViewModel>().ReverseMap();
            CreateMap<ContatoEmpresa, ContatoEmpresaViewModel>()
               .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
               //.ForMember(origem => origem.Empresa, opt => opt.MapFrom(src => src.Empresa))
               .ReverseMap();
            CreateMap<FornecedorContato, ContatoFornecedorViewModel>()
       .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
       .ForMember(origem => origem.IDFORN, opt => opt.MapFrom(src => src.IDFORN))
       .ReverseMap();

            CreateMap<ClienteContato, ClienteContatoViewModel>()
            .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
            .ForMember(origem => origem.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
            .ReverseMap();
            #endregion

            #region Endereco
            CreateMap<Endereco, EnderecoIndexViewModel>()
                    .ForMember(origem => origem.NumeroCep, opt => opt.MapFrom(src => src.Cep))
                    .ForMember(origem => origem.Numero, opt => opt.MapFrom(src => src.Numero))
                    .ForMember(origem => origem.Uf, opt => opt.MapFrom(src => src.Uf))
                    .ForMember(origem => origem.Cidade, opt => opt.MapFrom(src => src.Cidade))
                    .ForMember(origem => origem.Bairro, opt => opt.MapFrom(src => src.Bairro))
                    .ForMember(origem => origem.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                    .ForMember(origem => origem.Complemento, opt => opt.MapFrom(src => src.Complemento))
                    .ForMember(origem => origem.Ibge, opt => opt.MapFrom(src => src.Ibge))
                    .ForMember(origem => origem.Pais, opt => opt.MapFrom(src => src.Pais))
                    .ForMember(origem => origem.Id, opt => opt.MapFrom(src => src.Id))
                    .ReverseMap();
            CreateMap<Cep, CepViewModel>().ReverseMap();
            #endregion

            #region Produto

            CreateMap<ProdutoFoto, ProdutoFotoViewModel>()
               .ForMember(dest => dest.Foto, act => act.Ignore())
          .ReverseMap();
            CreateMap<agilium.api.business.Models.Produto, ListaProdutos>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMPRODUTO))
          .ReverseMap();
            
            CreateMap<agilium.api.business.Models.Produto, ProdutoViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPRODUTO))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMPRODUTO))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STPRODUTO))
                .ForMember(dest => dest.QuantMinima, opt => opt.MapFrom(src => src.NUQTDMIN))
                .ForMember(dest => dest.PCIBPTEST, opt => opt.MapFrom(src => src.PCIBPTEST))
                .ForMember(dest => dest.STESTOQUE, opt => opt.MapFrom(src => src.STESTOQUE))
                .ForMember(dest => dest.AliquotaCofins, opt => opt.MapFrom(src => src.PCCOFINS_ALIQ))
                .ForMember(dest => dest.AliquotaICMS, opt => opt.MapFrom(src => src.PCICMS_ALIQ))
                .ForMember(dest => dest.AliquotaICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_ALIQ))
                .ForMember(dest => dest.AliquotaIPI, opt => opt.MapFrom(src => src.PCIPI_ALIQ))
                .ForMember(dest => dest.AliquotaMargemValorAgregadoICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_MVA))
                .ForMember(dest => dest.AliquotaPIS, opt => opt.MapFrom(src => src.PCPIS_ALIQ))
                .ForMember(dest => dest.DSICMS_CST, opt => opt.MapFrom(src => src.DSICMS_CST))
                //.ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.CTPRODUTO))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom((src, dest) =>
                {
                    if (src.CTPRODUTO == "2")
                        return ECategoriaProduto.Composto;
                    if (src.CTPRODUTO == "3")
                        return ECategoriaProduto.Combo;
                    else if (src.CTPRODUTO == "4")
                        return ECategoriaProduto.Insumo;
                    else return ECategoriaProduto.Simples;
                }))
                .ForMember(dest => dest.CFOPVenda, opt => opt.MapFrom(src => src.NUCFOP))
                .ForMember(dest => dest.CodigoANP, opt => opt.MapFrom(src => src.CDANP))
                .ForMember(dest => dest.CodigoCest, opt => opt.MapFrom(src => src.CDCEST))
                .ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.CDNCM))
                .ForMember(dest => dest.CodigoSefaz, opt => opt.MapFrom(src => src.CDSEFAZ))
                .ForMember(dest => dest.CodigoServ, opt => opt.MapFrom(src => src.CDSERV))
                .ForMember(dest => dest.CodigoSituacaoTributariaCofins, opt => opt.MapFrom(src => src.DSCOFINS_CST))
                .ForMember(dest => dest.CodigoSituacaoTributariaIPI, opt => opt.MapFrom(src => src.DSIPI_CST))
                .ForMember(dest => dest.CodigoSituacaoTributariaPIS, opt => opt.MapFrom(src => src.DSPIS_CST))
                .ForMember(dest => dest.FLG_IFOOD, opt => opt.MapFrom(src => src.FLG_IFOOD))
                .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.NUPRECO))
                .ForMember(dest => dest.ReducaoBaseCalculoICMS, opt => opt.MapFrom(src => src.PCICMS_REDUCBC))
                .ForMember(dest => dest.ReducaoBaseCalculoICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_REDUCBC))
                .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
                .ForMember(dest => dest.ValorUltimaCompra, opt => opt.MapFrom(src => src.VLULTIMACOMPRA))
                .ForMember(dest => dest.RelacaoCompraVenda, opt => opt.MapFrom(src => src.NURELACAO))
                .ForMember(dest => dest.UtilizaBalanca, opt => opt.MapFrom(src => src.STBALANCA))
                .ForMember(dest => dest.UnidadeVenda, opt => opt.MapFrom(src => src.UNVENDA))
                .ForMember(dest => dest.UnidadeCompra, opt => opt.MapFrom(src => src.UNCOMPRA))
                .ForMember(dest => dest.PCIBPTFED, opt => opt.MapFrom(src => src.PCIBPTFED))
                .ForMember(dest => dest.PCIBPTIMP, opt => opt.MapFrom(src => src.PCIBPTIMP))
                .ForMember(dest => dest.PCIBPTMUN, opt => opt.MapFrom(src => src.PCIBPTMUN))
                .ForMember(dest => dest.IDDEP, opt => opt.MapFrom(src => src.IDDEP))
                .ForMember(dest => dest.IDGRUPO, opt => opt.MapFrom(src => src.IDGRUPO))
                .ForMember(dest => dest.IDSUBGRUPO, opt => opt.MapFrom(src => src.IDSUBGRUPO))
                .ForMember(dest => dest.IDMARCA, opt => opt.MapFrom(src => src.IDMARCA))
                .ForMember(dest => dest.idEmpresa, opt => opt.MapFrom(src => src.idEmpresa))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPPRODUTO))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.DSVOLUME))
                .ForMember(dest => dest.Empresas, act => act.Ignore())
                .ForMember(dest => dest.Cfops, act => act.Ignore())
                .ForMember(dest => dest.Csts, act => act.Ignore())
                .ForMember(dest => dest.Cests, act => act.Ignore())
                .ReverseMap();

            CreateMap<ProdutoDepartamento, ProdutoDepartamentoViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDDEP))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMDEP))
                .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STDEP))
                .ForMember(dest => dest.Empresas, act => act.Ignore())
                .ReverseMap();
            CreateMap<ProdutoMarca, ProdutoMarcaViewModel>()
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDMARCA))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMMARCA))
              .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STMARCA))
              .ForMember(dest => dest.Empresas, act => act.Ignore())
              .ReverseMap();
            CreateMap<GrupoProduto, GrupoProdutoViewModel>()
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDGRUPO))
             .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.StAtivo))
             .ForMember(dest => dest.Empresas, act => act.Ignore())
             .ReverseMap();
            CreateMap<SubGrupoProduto, SubGrupoViewModel>()
             .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMSUBGRUPO))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STATIVO))
             .ForMember(dest => dest.NomeGrupo, act => act.Ignore())
             .ReverseMap();

            CreateMap<ProdutoCodigoBarra, ProdutoCodigoBarraViewModel>()
                .ForMember(dest => dest.CDBARRA, opt => opt.MapFrom(src => src.CDBARRA))
                .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
             .ReverseMap();

            CreateMap<ProdutoPreco, ProdutoPrecoViewModel>()
                .ReverseMap();


            #endregion

            #region Devolucao
            CreateMap<MotivoDevolucao, MotivoDevolucaoViewModel>()
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOTDEV))
               .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STMOTDEV))
               .ForMember(dest => dest.Empresas, act => act.Ignore())
               .ReverseMap();

            CreateMap<Devolucao, DevolucaoViewModel>()
             .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
             .ForMember(dest => dest.IDVALE, opt => opt.MapFrom(src => src.IDVALE))
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDDEV))
             .ForMember(dest => dest.IDMOTDEV, opt => opt.MapFrom(src => src.IDMOTDEV))
             .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBSDEV))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRDEV))
             .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                    .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STDEV))
             .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTALDEV))
             .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
             .ForMember(dest => dest.ValeNome, act => act.Ignore())
             .ForMember(dest => dest.MotivoDevolucaoNome, act => act.Ignore())
             .ForMember(dest => dest.ClienteNome, act => act.Ignore())
             .ForMember(dest => dest.Itens, act => act.Ignore())
             .ForMember(dest => dest.VendaNome, act => act.Ignore())
             .ForMember(dest => dest.DevolucaoItens, act => act.Ignore())
               .ReverseMap();

            CreateMap<DevolucaoItem, DevolucaoItemViewModel>()
            .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
            .ForMember(dest => dest.IDDEV, opt => opt.MapFrom(src => src.IDDEV))
            .ForMember(dest => dest.IDVENDA_ITEM, opt => opt.MapFrom(src => src.IDVENDA_ITEM))
            .ForMember(dest => dest.ValorItem, opt => opt.MapFrom(src => src.VLITEM))
            .ForMember(dest => dest.DevolucaoNome, act => act.Ignore())
            .ForMember(dest => dest.VendaItemNome, act => act.Ignore())
             .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
              .ForMember(dest => dest.ValorItemVenda, act => act.Ignore())
               .ForMember(dest => dest.SequencialVenda, act => act.Ignore())
            .ReverseMap();

            CreateMap<DevolucaoItemVendaCustom, DevolucaoItemVendaViewModel>()
                .ReverseMap();
            #endregion

            #region Cliente
            CreateMap<Cliente, ClienteViewModel>()
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCLIENTE))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCLIENTE))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMCLIENTE))
              .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPCLIENTE))
              .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DTCAD))
              .ForMember(dest => dest.PublicaEmail, opt => opt.MapFrom(src => src.STPUBEMAIL))
              .ForMember(dest => dest.PublicaSMS, opt => opt.MapFrom(src => src.STPUBSMS))
              .ForMember(dest => dest.IDENDERECO, opt => opt.MapFrom(src => src.IDENDERECO))
              .ForMember(dest => dest.IDENDERECOCOB, opt => opt.MapFrom(src => src.IDENDERECOCOB))
              .ForMember(dest => dest.IDENDERECOFAT, opt => opt.MapFrom(src => src.IDENDERECOFAT))
              .ForMember(dest => dest.IDENDERECONTREGA, opt => opt.MapFrom(src => src.IDENDERECONTREGA))
              .ForMember(dest => dest.Endereco, act => act.Ignore())
              .ForMember(dest => dest.EnderecoCobranca, act => act.Ignore())
              .ForMember(dest => dest.EnderecoEntrega, act => act.Ignore())
              .ForMember(dest => dest.EnderecoFaturamento, act => act.Ignore())
              .ForMember(dest => dest.ClientePessoaFisica, act => act.Ignore())
              .ForMember(dest => dest.ClientePessoaJuridica, act => act.Ignore())
              .ReverseMap();

            CreateMap<ClientePF, ClientePFViewModel>()
              .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.NUCPF))
              .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NURG))
              .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DTNASC))
              .ReverseMap();

            CreateMap<ClientePJ, ClientePJViewModel>()
              .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.NUCNPJ))
              .ForMember(dest => dest.InscricaoEstadual, opt => opt.MapFrom(src => src.DSINSCREST))
              .ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.NMRZSOCIAL))
              .ReverseMap();

            CreateMap<ClientePreco, ClientePrecoViewModel>()
                 .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                 .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
                 .ForMember(dest => dest.Diferenca, opt => opt.MapFrom(src => src.TPDIFERENCA))
                 .ForMember(dest => dest.TipoValor, opt => opt.MapFrom(src => src.TPVALOR))
                 .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.NUVALOR))
                 .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.NmUsuario))
                 .ForMember(dest => dest.Datahora, opt => opt.MapFrom(src => src.DTHRCAD))
                .ReverseMap();
            #endregion

            #region Estoque
            CreateMap<Estoque, EstoqueViewModel>()
              .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STESTOQUE))
              .ForMember(dest => dest.Empresas, act => act.Ignore())
              .ReverseMap();

            CreateMap<EstoqueProduto, EstoqueProdutoViewModel>()
                .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
                .ReverseMap();

            CreateMap<EstoqueHistorico, EstoqueHistoricoViewModel>()
                  .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRHST))
                  .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
                  .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.QTDHST))
                  .ForMember(dest => dest.IDITEM, opt => opt.MapFrom(src => src.IDITEM))
                  .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSHST))
                  .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
                  .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.NMUSUARIO))
                  .ForMember(dest => dest.TipoHistorico, opt => opt.MapFrom(src => src.TPHST))
                .ReverseMap();

            CreateMap<agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel.EstoquePosicaoReport, agilum.mvc.web.ViewModels.Estoque.EstoquePosicaoReport>()
                    .ReverseMap();
            #endregion

            #region Fornecedor
            CreateMap<Fornecedor, FornecedorViewModel>()
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFORNEC))
               .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.NMFANTASIA))
               .ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.NMRZSOCIAL))
               .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPPESSOA))
                     .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPPESSOA == "J" ? ETipoPessoa.J : ETipoPessoa.F))
                
               .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.NUCPFCNPJ))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDFORN))
               .ForMember(dest => dest.InscricaoEstdualMunicipal, opt => opt.MapFrom(src => src.DSINSCR))
                 .ForMember(dest => dest.TipoFiscal, opt => opt.MapFrom(src => src.TPFISCAL))
                 .ForMember(dest => dest.Endereco, act => act.Ignore())
                   .ForMember(dest => dest.Contatos, act => act.Ignore())
               .ReverseMap();
            #endregion

            #region Funcionario
            CreateMap<Funcionario, FuncionarioViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDFUNC))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMFUNC))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFUNC))
                .ForMember(dest => dest.Noturno, opt => opt.MapFrom(src => src.NUTURNO))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.NUCPF))
                .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.NURG))
                .ForMember(dest => dest.DataAdmissao, opt => opt.MapFrom(src => src.DTADM))
                .ForMember(dest => dest.DataDemissao, opt => opt.MapFrom(src => src.DTDEM))
                .ForMember(dest => dest.DSRFID, opt => opt.MapFrom(src => src.DSRFID))
                .ForMember(dest => dest.Turno, opt => opt.MapFrom(src => src.STNOTURNO))
                 //.ForMember(dest => dest.IDENDERECO, act => act.Ignore())
                 .ForMember(dest => dest.Empresas, act => act.Ignore())
                  //  .ForMember(dest => dest.Usuario, act => act.Ignore())
                  .ReverseMap();
            #endregion

            #region Impostos
            CreateMap<Cst, CstViewModel>().ReverseMap();
            CreateMap<Csosn, CsosnViewModel>().ReverseMap();
            CreateMap<CestNcm, CestViewModel>().ReverseMap();
            CreateMap<Ncm, NcmViewModel>().ReverseMap();
            CreateMap<Ibpt, IbptViewModel>().ReverseMap();
            CreateMap<Cfop, CfopViewModel>().ReverseMap();
            #endregion

            #region Turno

            CreateMap<Turno, TurnoIndexViewModel>()
             .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.IDUSUARIOFIM, opt => opt.MapFrom(src => src.IDUSUARIOFIM))
             .ForMember(dest => dest.IDUSUARIOINI, opt => opt.MapFrom(src => src.IDUSUARIOINI))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTTURNO))
             .ForMember(dest => dest.DataFinal, opt => opt.MapFrom(src => src.DTHRFIM))
             .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DTHRINI))
             .ForMember(dest => dest.Obs, opt => opt.MapFrom(src => src.DSOBS))
             .ForMember(dest => dest.Empresa, act => act.Ignore())
             .ForMember(dest => dest.UsuarioFinal, act => act.Ignore())
             .ForMember(dest => dest.UsuarioInicial, act => act.Ignore())
            .ReverseMap();

            CreateMap<TurnoPreco, TurnoPrecoViewModel>()
          .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
          .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
          .ForMember(dest => dest.Diferenca, opt => opt.MapFrom(src => src.TPDIFERENCA))
          .ForMember(dest => dest.TipoValor, opt => opt.MapFrom(src => src.TPVALOR))
          .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.NUVALOR))
          .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.NMUSUARIO))
          .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRCAD))
           .ForMember(dest => dest.DescricaoTipoValor, act => act.Ignore())
            .ForMember(dest => dest.DescricaoTipoDiferenca, act => act.Ignore())
             .ForMember(dest => dest.NomeCliente, act => act.Ignore())
         .ForMember(dest => dest.ValorFinal, act => act.Ignore())
          .ForMember(dest => dest.Clientes, act => act.Ignore())
         .ReverseMap();
            #endregion

            #region PDV
            CreateMap<PontoVenda, PontoVendaViewModel>()
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPDV))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSPDV))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STPDV))
               .ForMember(dest => dest.NomeMaquina, opt => opt.MapFrom(src => src.NMMAQUINA))
               .ForMember(dest => dest.CaminhoCertificadoDigital, opt => opt.MapFrom(src => src.DSCAMINHO_CERT))
               .ForMember(dest => dest.SenhaCertificadoDigital, opt => opt.MapFrom(src => src.DSSENHA_CERT))
               .ForMember(dest => dest.PortaImpressora, opt => opt.MapFrom(src => src.DSPORTAIMPRESSORA))
               .ForMember(dest => dest.NUBAUDRATEBAL, opt => opt.MapFrom(src => src.NUBAUDRATEBAL))
               .ForMember(dest => dest.CDHANDSHAKEBAL, opt => opt.MapFrom(src => src.CDHANDSHAKEBAL))
               .ForMember(dest => dest.CDPARITYBAL, opt => opt.MapFrom(src => src.CDPARITYBAL))
               .ForMember(dest => dest.CDMODELOBAL, opt => opt.MapFrom(src => src.CDMODELOBAL))
               .ForMember(dest => dest.CDSERIALSTOPBITBAL, opt => opt.MapFrom(src => src.CDSERIALSTOPBITBAL))
               .ForMember(dest => dest.DSPORTABAL, opt => opt.MapFrom(src => src.DSPORTABAL))
               // .ForMember(dest => dest.Estoques, opt => opt.MapFrom(src => src.Estoque))
               .ForMember(dest => dest.Estoques, act => act.Ignore())
               .ForMember(dest => dest.Empresas, act => act.Ignore())
             .ReverseMap();
            #endregion

            #region Moeda
            CreateMap<Moeda, MoedaViewModel>()
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDMOEDA))
             .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOEDA))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STMOEDA))
             .ForMember(dest => dest.SitucacaoTroco, opt => opt.MapFrom(src => src.STTROCO))
             .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPMOEDA))
             .ForMember(dest => dest.TipoDocFiscal, opt => opt.MapFrom(src => src.TPDOCFISCAL))
             .ForMember(dest => dest.PorcentTaxa, opt => opt.MapFrom(src => src.PCTAXA))
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.COR_BOTAO, opt => opt.MapFrom(src => src.COR_BOTAO))
             .ForMember(dest => dest.COR_FONTE, opt => opt.MapFrom(src => src.COR_FONTE))
             .ForMember(dest => dest.TECLA_ATALHO, opt => opt.MapFrom(src => src.TECLA_ATALHO))
              //.ForMember(dest => dest.IDENDERECO, act => act.Ignore())
              .ForMember(dest => dest.Empresas, act => act.Ignore())
               //  .ForMember(dest => dest.Usuario, act => act.Ignore())
               .ReverseMap();
            #endregion

            #region forma pagamento
            CreateMap<FormaPagamento, FormaPagamentoViewModel>()
               .ForMember(dest => dest.IDEmpresa, opt => opt.MapFrom(src => src.IDEmpresa))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFormaPagamento))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSFormaPagamento))
               .ReverseMap();
            #endregion

            #region Compra
            CreateMap<Compra, CompraViewModel>()
            .ForMember(dest => dest.IDFORN, opt => opt.MapFrom(src => src.IDFORN))
            .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
            .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCOMPRA))
            .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCOMPRA))
            .ForMember(dest => dest.SerieNF, opt => opt.MapFrom(src => src.DSSERIENF))
            .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DTCAD))
            .ForMember(dest => dest.NumeroNF, opt => opt.MapFrom(src => src.NUNF))
            .ForMember(dest => dest.DataNF, opt => opt.MapFrom(src => src.DTNF))
            .ForMember(dest => dest.DataCompra, opt => opt.MapFrom(src => src.DTCOMPRA))
            .ForMember(dest => dest.ValorIcmsSub, opt => opt.MapFrom(src => src.VLICMSSUB))
            .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
            .ForMember(dest => dest.Importada, opt => opt.MapFrom(src => src.STIMPORTADA))
            .ForMember(dest => dest.NumeroCFOP, opt => opt.MapFrom(src => src.NUCFOP))
            .ForMember(dest => dest.TipoComprovante, opt => opt.MapFrom(src => src.TPCOMPROVANTE))
            .ForMember(dest => dest.ValorBaseCalculoIcms, opt => opt.MapFrom(src => src.VLBSCALCICMS))
            .ForMember(dest => dest.ValorBaseCalculoSub, opt => opt.MapFrom(src => src.VLBSCALCSUB))
            .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESCONTO))
            .ForMember(dest => dest.ValorFrete, opt => opt.MapFrom(src => src.VLFRETE))
            .ForMember(dest => dest.ValorIcms, opt => opt.MapFrom(src => src.VLICMS))
            .ForMember(dest => dest.ValorIcmsRetido, opt => opt.MapFrom(src => src.VLICMSRETIDO))
            .ForMember(dest => dest.ValorIpi, opt => opt.MapFrom(src => src.VLIPI))
            .ForMember(dest => dest.ValorIsencao, opt => opt.MapFrom(src => src.VLISENCAO))
            .ForMember(dest => dest.ValorOutros, opt => opt.MapFrom(src => src.VLOUTROS))
            .ForMember(dest => dest.ValorSeguro, opt => opt.MapFrom(src => src.VLSEGURO))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
            .ForMember(dest => dest.ValorTotalProduto, opt => opt.MapFrom(src => src.VLTOTPROD))
            .ForMember(dest => dest.ChaveNFE, opt => opt.MapFrom(src => src.DSCHAVENFE))

            .ForMember(dest => dest.NomeFornecedor, act => act.Ignore())
            .ForMember(dest => dest.NomeTurno, act => act.Ignore())
            .ReverseMap();

            CreateMap<CompraItem, CompraItemViewModel>()
              .ForMember(dest => dest.IDCOMPRA, opt => opt.MapFrom(src => src.IDCOMPRA))
              .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
              .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
              .ForMember(dest => dest.ValorAliquotaCofins, opt => opt.MapFrom(src => src.VLALIQCOFINS))
              .ForMember(dest => dest.ValorAliquotaIcms, opt => opt.MapFrom(src => src.VLALIQICMS))
              .ForMember(dest => dest.ValorAliquotaIpi, opt => opt.MapFrom(src => src.VLALIQIPI))
              .ForMember(dest => dest.ValorAliquotaPis, opt => opt.MapFrom(src => src.VLALIQPIS))
              .ForMember(dest => dest.ValorBaseCalculoCofins, opt => opt.MapFrom(src => src.VLBSCALCCOFINS))
              .ForMember(dest => dest.ValorBaseCalculoIcms, opt => opt.MapFrom(src => src.VLBSCALCICMS))
              .ForMember(dest => dest.ValorBaseCalculoIpi, opt => opt.MapFrom(src => src.VLBSCALCIPI))
              .ForMember(dest => dest.ValorBaseCalculoPis, opt => opt.MapFrom(src => src.VLBSCALCPIS))
              .ForMember(dest => dest.ValorBaseRetido, opt => opt.MapFrom(src => src.VLBSRET))
              .ForMember(dest => dest.ValorCofins, opt => opt.MapFrom(src => src.VLCOFINS))
              .ForMember(dest => dest.ValorIcms, opt => opt.MapFrom(src => src.VLICMS))
              .ForMember(dest => dest.ValorIpi, opt => opt.MapFrom(src => src.VLIPI))
              .ForMember(dest => dest.ValorNovoPrecoVenda, opt => opt.MapFrom(src => src.VLNOVOPRECOVENDA))
              .ForMember(dest => dest.ValorOUTROS, opt => opt.MapFrom(src => src.VLOUTROS))
              .ForMember(dest => dest.ValorPis, opt => opt.MapFrom(src => src.VLPIS))
              .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
              .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.VLUNIT))
              .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
              .ForMember(dest => dest.CodigoCEST, opt => opt.MapFrom(src => src.CDCEST))
              .ForMember(dest => dest.CodigoCstCofins, opt => opt.MapFrom(src => src.CDCSTCOFINS))
              .ForMember(dest => dest.CodigoCstIcms, opt => opt.MapFrom(src => src.CDCSTICMS))
              .ForMember(dest => dest.CodigoCstIpi, opt => opt.MapFrom(src => src.CDCSTIPI))
              .ForMember(dest => dest.CodigoCstPis, opt => opt.MapFrom(src => src.CDCSTPIS))
              .ForMember(dest => dest.CodigoEan, opt => opt.MapFrom(src => src.CDEAN))
              .ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.CDNCM))
              .ForMember(dest => dest.CodigoProdutoFornecedor, opt => opt.MapFrom(src => src.CDPRODFORN))
              .ForMember(dest => dest.PorcentagemIcmsRetido, opt => opt.MapFrom(src => src.PCICMSRET))
              .ForMember(dest => dest.PorcentagemReducao, opt => opt.MapFrom(src => src.PCREDUCAO))
              .ForMember(dest => dest.DataValidade, opt => opt.MapFrom(src => src.DTVALIDADE))
              .ForMember(dest => dest.SGUN, opt => opt.MapFrom(src => src.SGUN))
              .ForMember(dest => dest.NumeroCFOP, opt => opt.MapFrom(src => src.NUCFOP))
              .ForMember(dest => dest.Relacao, opt => opt.MapFrom(src => src.NURELACAO))
              .ForMember(dest => dest.DescricaoProdutoCompra, opt => opt.MapFrom(src => src.DSPRODUTO))
              .ForMember(dest => dest.NomeProduto, act => act.Ignore())
              .ForMember(dest => dest.NomeCompra, act => act.Ignore())
              .ForMember(dest => dest.NomeEstoque, act => act.Ignore())
               .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
              .ReverseMap();

            CreateMap<CompraFiscal, CompraFiscalViewModel>()
            .ForMember(dest => dest.IDCOMPRA, opt => opt.MapFrom(src => src.IDCOMPRA))
            .ForMember(dest => dest.TipoManifesto, opt => opt.MapFrom(src => src.STMANIFESTO))
            .ForMember(dest => dest.Xml, opt => opt.MapFrom(src => src.DSXML))
            .ReverseMap();

            CreateMap<CompraIndexViewModelReturn, CompraIndexViewModel>()
                .ReverseMap();
            #endregion

            #region Perda
            CreateMap<Perda, PerdaViewModel>()
              .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
              .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
              .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
              .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
              .ForMember(dest => dest.IDESTOQUEHST, opt => opt.MapFrom(src => src.IDESTOQUEHST))
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPERDA))
              .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRPERDA))
              .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPPERDA))
              .ForMember(dest => dest.Movimento, opt => opt.MapFrom(src => src.TPMOV))
              .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
              .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
              .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTDPERDA))
              .ForMember(dest => dest.EstoqueNome, act => act.Ignore())
              .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
              .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
              .ForMember(dest => dest.EstoqueHistoricoNome, act => act.Ignore())
              .ForMember(dest => dest.UsuarioNome, act => act.Ignore())
             .ReverseMap();
            #endregion

            #region Inventario
            CreateMap<Inventario, InventarioViewModel>()
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.TipoAnalise, opt => opt.MapFrom(src => src.TPANALISE))
             .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STINVENT))
             .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDINVENT))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTINVENT))
              .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSINVENT))
             .ForMember(dest => dest.NomeEstoque, act => act.Ignore())
             .ReverseMap();

            CreateMap<InventarioItem, InventarioItemViewModel>()
             .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
             .ForMember(dest => dest.IDINVENT, opt => opt.MapFrom(src => src.IDINVENT))
             .ForMember(dest => dest.IDPERDA, opt => opt.MapFrom(src => src.IDPERDA))
             .ForMember(dest => dest.IDUSUARIOANALISE, opt => opt.MapFrom(src => src.IDUSUARIOANALISE))
             .ForMember(dest => dest.QuantidadeEstoque, opt => opt.MapFrom(src => src.NUQTDESTOQUE))
             .ForMember(dest => dest.QuantidadeAnalise, opt => opt.MapFrom(src => src.NUQTDANALISE))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRANALISE))
             .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
             .ForMember(dest => dest.NomePerda, act => act.Ignore())
             .ForMember(dest => dest.NomeProduto, act => act.Ignore())
             .ForMember(dest => dest.NomeUsuarioAnalise, act => act.Ignore())
              .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
             .ReverseMap();

            #endregion

            #region Caixa
            CreateMap<Caixa, CaixaViewModel>()
              .ForMember(dest => dest.IDFUNC, opt => opt.MapFrom(src => src.IDFUNC))
              .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
              .ForMember(dest => dest.IDPDV, opt => opt.MapFrom(src => src.IDPDV))
              .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQCAIXA))
              .ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.DTHRABT))
              .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(src => src.DTHRFECH))
              .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
              .ForMember(dest => dest.ValorAbertura, opt => opt.MapFrom(src => src.VLABT))
              .ForMember(dest => dest.ValorFechamento, opt => opt.MapFrom(src => src.VLFECH))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCAIXA))
             .ReverseMap();

            CreateMap<Caixa, CaixaindexViewModel>()
                .ForMember(dest => dest.IDFUNC, opt => opt.MapFrom(src => src.IDFUNC))
                .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                .ForMember(dest => dest.IDPDV, opt => opt.MapFrom(src => src.IDPDV))
                .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQCAIXA))
                .ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.DTHRABT))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(src => src.DTHRFECH))
                .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
                .ForMember(dest => dest.ValorAbertura, opt => opt.MapFrom(src => src.VLABT))
                .ForMember(dest => dest.ValorFechamento, opt => opt.MapFrom(src => src.VLFECH))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCAIXA))
                .ForMember(dest => dest.Empresa, act => act.Ignore())
                .ForMember(dest => dest.Funcionario, act => act.Ignore())
                .ForMember(dest => dest.Turno, act => act.Ignore())
                .ForMember(dest => dest.PDV, act => act.Ignore())
               .ReverseMap();

            CreateMap<CaixaMovimento, CaixaMovimentoViewModel>()
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPMOV))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STMOV))
             .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOV))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLMOV))
             .ForMember(dest => dest.Caixa, act => act.Ignore())
             .ReverseMap();

            CreateMap<CaixaMoeda, CaixaMoedaViewModel>()
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.IDMOEDA, opt => opt.MapFrom(src => src.IDMOEDA))
             .ForMember(dest => dest.IDUSUARIOCORRECAO, opt => opt.MapFrom(src => src.IDUSUARIOCORRECAO))
             .ForMember(dest => dest.DataCorrecao, opt => opt.MapFrom(src => src.DTHRCORRECAO))
             .ForMember(dest => dest.ValorCorrecao, opt => opt.MapFrom(src => src.VLMOEDACORRECAO))
             .ForMember(dest => dest.ValorOriginal, opt => opt.MapFrom(src => src.VLMOEDAORIGINAL))
             .ForMember(dest => dest.CaixaNome, act => act.Ignore())
             .ForMember(dest => dest.MoedaNome, act => act.Ignore())
             .ReverseMap();

            #endregion

            #region Venda
            CreateMap<Venda, VendaViewModel>()
             .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.NumeroNF, opt => opt.MapFrom(src => src.NUNF))
             .ForMember(dest => dest.InformacaoComplementar, opt => opt.MapFrom(src => src.DSINFCOMPL))
             .ForMember(dest => dest.SerieNF, opt => opt.MapFrom(src => src.DSSERIE))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STVENDA))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLVENDA))
             .ForMember(dest => dest.ChaveAcesso, opt => opt.MapFrom(src => src.DSCHAVEACESSO))
             .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.NUCPFCNPJ))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTHRVENDA))
             .ForMember(dest => dest.Emissao, opt => opt.MapFrom(src => src.STEMISSAO))
             .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQVENDA))
             .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TPDOC))
             .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
             .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
             .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
             .ForMember(dest => dest.ValorTotalIbptEst, opt => opt.MapFrom(src => src.VLTOTIBPTEST))
             .ForMember(dest => dest.ValorTotalIbptFed, opt => opt.MapFrom(src => src.VLTOTIBPTFED))
             .ForMember(dest => dest.ValorTotalIbptImp, opt => opt.MapFrom(src => src.VLTOTIBPTIMP))
             .ForMember(dest => dest.ValorTotalIbptMun, opt => opt.MapFrom(src => src.VLTOTIBPTMUN))
             .ForMember(dest => dest.CaixaNome, act => act.Ignore())
             .ForMember(dest => dest.ClienteNome, act => act.Ignore())
             .ForMember(dest => dest.PDVNome, act => act.Ignore())
             .ForMember(dest => dest.FuncionarioNome, act => act.Ignore())
            .ReverseMap();

            CreateMap<VendaItem, VendaItemViewModel>()
               .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
               .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
               .ForMember(dest => dest.PCIBPTEST, opt => opt.MapFrom(src => src.PCIBPTEST))
               .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
               .ForMember(dest => dest.PCIBPTFED, opt => opt.MapFrom(src => src.PCIBPTFED))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STITEM))
               .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLITEM))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
               .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.PCIBPTIMP, opt => opt.MapFrom(src => src.PCIBPTIMP))
               .ForMember(dest => dest.PCIBPTMUN, opt => opt.MapFrom(src => src.PCIBPTMUN))
               .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQITEM))
               .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
               .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.VLUNIT))
               .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
                .ForMember(dest => dest.VendaNome, act => act.Ignore())
                .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
                .ForMember(dest => dest.SituacaoProduto, act => act.Ignore())
              .ReverseMap();

            CreateMap<VendaMoeda, VendaMoedaViewModel>()
                .ForMember(dest => dest.IDMOEDA, opt => opt.MapFrom(src => src.IDMOEDA))
                .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
                .ForMember(dest => dest.IDVALE, opt => opt.MapFrom(src => src.IDVALE))
                .ForMember(dest => dest.NSU, opt => opt.MapFrom(src => src.NSU))
                .ForMember(dest => dest.ValorTroco, opt => opt.MapFrom(src => src.VLTROCO))
                .ForMember(dest => dest.NumeroParcela, opt => opt.MapFrom(src => src.NUPARCELAS))
                .ForMember(dest => dest.ValorPago, opt => opt.MapFrom(src => src.VLPAGO))
                .ForMember(dest => dest.MoedaNome, act => act.Ignore())
                .ForMember(dest => dest.VendaNome, act => act.Ignore())
               .ReverseMap();

            CreateMap<VendaEspelho, VendaEspelhoViewModel>()
            .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
            .ForMember(dest => dest.EspelhoVenda, opt => opt.MapFrom(src => src.DSESPELHO))
            .ForMember(dest => dest.SequencialVenda, act => act.Ignore())
              .ReverseMap();


            #endregion

            #region Venda Report
            CreateMap<agilum.mvc.web.ViewModels.Venda.VendasReportViewModel, agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel.VendasReportViewModel>()
                  .ReverseMap();
            
            #endregion

            #region Vale
            CreateMap<Vale, ValeViewModel>()
               .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDVALE))
               .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRVALE))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPVALE))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STVALE))
               .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLVALE))
               .ForMember(dest => dest.CodigoBarra, opt => opt.MapFrom(src => src.CDBARRA))
               .ForMember(dest => dest.ClienteNome, act => act.Ignore())
               .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
              .ReverseMap();
            #endregion

            #region Plano Conta
            CreateMap<PlanoConta, PlanoContaViewModel>()
                  .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
                  .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                  .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCONTA))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSCONTA))
                  .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPCONTA))
                  .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
                 .ReverseMap();

            CreateMap<PlanoContaSaldo, PlanoContaSaldoViewModel>()
                .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
                .ForMember(dest => dest.AnoMesReferencia, opt => opt.MapFrom(src => src.NUANOMESREF))
                .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRATU))
                .ForMember(dest => dest.ValorSaldo, opt => opt.MapFrom(src => src.VLSALDO))
                .ReverseMap();

            CreateMap<PlanoContaLancamento, PlanoContaLancamentoViewModel>()
             .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
             .ForMember(dest => dest.AnoMesReferencia, opt => opt.MapFrom(src => src.NUANOMESREF))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTCAD))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLLANC))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STLANC))
               .ForMember(dest => dest.DataReferencia, opt => opt.MapFrom(src => src.DTREF))
               .ForMember(dest => dest.DescricaoLancamento, opt => opt.MapFrom(src => src.DSLANC))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPLANC))
             .ReverseMap();
            #endregion

            #region CategoriaFinanceira
            CreateMap<CategoriaFinanceira, CategeoriaFinanceiraViewModel>().ReverseMap();
            #endregion

            #region Conta
            CreateMap<ContaPagar, ContaPagarViewModel>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDFORNEC, opt => opt.MapFrom(src => src.IDFORNEC))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRESC))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
              .ReverseMap();

            CreateMap<ContaPagar, ContaPagarViewModelIndex>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDFORNEC, opt => opt.MapFrom(src => src.IDFORNEC))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRESC))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
               .ForMember(dest => dest.CategoriaFinanceira, act => act.Ignore())
               .ForMember(dest => dest.Conta, act => act.Ignore())
                .ForMember(dest => dest.Fornecedor, act => act.Ignore())
                .ForMember(dest => dest.Usuario, act => act.Ignore())
              .ReverseMap();

            CreateMap<ContaReceber, ContaReceberViewModelIndex>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.CategoriaFinanceira, act => act.Ignore())
               .ForMember(dest => dest.Conta, act => act.Ignore())
                .ForMember(dest => dest.Cliente, act => act.Ignore())
                .ForMember(dest => dest.Usuario, act => act.Ignore())
              .ReverseMap();

            CreateMap<ContaReceber, ContaReceberViewModel>()
                .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
                .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
                .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
                .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
                .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
                .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
                .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
                .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
                .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
                        .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
                .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
                .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
                .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
                .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
                .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
                .ReverseMap();

            CreateMap<ContaPagarViewModel, ContaPagarViewModelIndex>().ReverseMap();
            #endregion

            #region Config
            CreateMap<Config, ConfigIndexViewModel>().ReverseMap();
            CreateMap<ConfigImagem, ConfigImagemViewModel>().ReverseMap();
            CreateMap<Config, ChaveValorViewModel>().ReverseMap();
            CreateMap<Config, EditarChaveValorViewModel>().ReverseMap();
            #endregion

            #region Log
            CreateMap<LogSistema, LogSistemaViewModel>()
                      .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_log))
                      .ForMember(dest => dest.Maquina, opt => opt.MapFrom(src => src.maquina))
                      .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data_log))
                      .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.descr))
                      .ForMember(dest => dest.AcaoOriem, opt => opt.MapFrom(src => src.controle))
                      .ForMember(dest => dest.TelaOrigem, opt => opt.MapFrom(src => src.tela))
                      .ForMember(dest => dest.so, opt => opt.MapFrom(src => src.so))
                      .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.hora_log))
                      .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.usuario))
                .ReverseMap();
            #endregion

            #region Licenca
            CreateMap<Licenca, LicencaViewModel>().ReverseMap();
            #endregion

        }
    }
}
