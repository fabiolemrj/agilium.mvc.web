using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace agilium.api.business.Services
{
    public class CompraService : BaseService, ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly ICompraItemRepository _compraItemRepository;
        private readonly ICompraFiscalRepository _compraFiscalRepository;
        private readonly ICompraDapperRepository _compraDapperRepository;
        private readonly IFornecedorDapperRepository _fornecedorDapperRepository;
        private readonly IContatoDapperRepository _contatoDapperRepository;
        private readonly IEnderecoDapperRepository _enderecoDapperRepository;
        private readonly IProdutoDapper _produtoDapperRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUtilDapperRepository _utilDapperRepository;
        private readonly IEstoqueDapperRepository _estoqueDapperRepository;
        private readonly IPlanoContaDapperRepository _planoContaDapperRepository;
        public CompraService(INotificador notificador, ICompraRepository compraRepository, ICompraItemRepository compraItemRepository,
            ICompraFiscalRepository compraFiscalRepository, ICompraDapperRepository compraDapperRepository, 
            IFornecedorDapperRepository fornecedorDapperRepository, IContatoDapperRepository contatoDapperRepository, IEnderecoDapperRepository enderecoDapperRepository,
            IProdutoDapper produtoDapper, IDapperRepository dapperRepository, IUtilDapperRepository utilDapperRepository, 
            IEstoqueDapperRepository estoqueDapperRepository, IPlanoContaDapperRepository planoContaDapperRepository) : base(notificador)
        {
            _compraRepository = compraRepository;
            _compraItemRepository = compraItemRepository;
            _compraFiscalRepository = compraFiscalRepository;
            _compraDapperRepository = compraDapperRepository;
            _fornecedorDapperRepository = fornecedorDapperRepository;
            _contatoDapperRepository = contatoDapperRepository;
            _enderecoDapperRepository = enderecoDapperRepository;
            _produtoDapperRepository = produtoDapper;
            _dapperRepository = dapperRepository;
            _utilDapperRepository = utilDapperRepository;
            _estoqueDapperRepository = estoqueDapperRepository;
            _planoContaDapperRepository = planoContaDapperRepository;
        }


        public async Task Salvar()
        {
            await _compraRepository.SaveChanges();
        }

        public void Dispose()
        {
            _compraFiscalRepository?.Dispose();
            _compraItemRepository?.Dispose();
            _compraRepository?.Dispose();
        }

        #region Compra
        public async Task Adicionar(Compra compra)
        {
            if (!ExecutarValidacao(new CompraValidation(), compra))
                return;

            await _compraRepository.AdicionarSemSalvar(compra);
        }

        public async Task Apagar(long id)
        {
            if (await PodeApagarCompra(id))
            {
                Notificar("Não é possivel apagar esta compra pois o mesmo está sendo utilizado");
                return;
            }
            await _compraRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Compra compra)
        {
            if (!ExecutarValidacao(new CompraValidation(), compra))
                return;

            await _compraRepository.AtualizarSemSalvar(compra);
        }

        public async Task<Compra> ObterPorId(long id)
        {
            return _compraRepository.ObterPorId(id).Result;
        }

        public async Task<IEnumerable<Compra>> ObterTodas(long idEmpresa)
        {
            return _compraRepository.Obter(x=>x.IDEMPRESA == idEmpresa).Result;
        }

        public async Task<PagedResult<Compra>> ObterCompraPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            
            var lista = _compraRepository.Obter(x=>x.IDEMPRESA == idEmpresa && x.DTCOMPRA >= dtIni && x.DTCOMPRA<= dtFim).Result;

            return new PagedResult<Compra>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }
        #endregion

        #region Item

        public async Task Adicionar(CompraItem compra)
        {
            if (!ExecutarValidacao(new CompraItemValidation(), compra))
                return;

            await _compraItemRepository.AdicionarSemSalvar(compra);
        }

        public async Task Atualizar(CompraItem compra)
        {
            if (!ExecutarValidacao(new CompraItemValidation(), compra))
                return;

            await _compraItemRepository.AtualizarSemSalvar(compra);
        }

        public async Task ApagarItem(long id)
        {
            if (await PodeApagarCompraItem(id))
            {
                Notificar("Não é possivel apagar este item da compra pois o mesmo está sendo utilizado");
                return;
            }
            await _compraItemRepository.RemoverSemSalvar(id);
        }

      
        public async Task<CompraItem> ObterItemPorId(long id)
        {
            return _compraItemRepository.ObterPorId(id).Result;
        }

        public async Task<List<CompraItem>> ObterItensPorCompra(long id)
        {
            return _compraItemRepository.Obter(x=>x.IDCOMPRA == id).Result.ToList();
        }

        #endregion

        #region Fiscal
        public async Task Adicionar(CompraFiscal compra)
        {
            if (!ExecutarValidacao(new CompraFiscalValidation(), compra))
                return;

            await _compraFiscalRepository.AdicionarSemSalvar(compra);
        }

        public async Task ApagarFiscal(long id)
        {
            if (await PodeApagarCompraFiscal(id))
            {
                Notificar("Não é possivel apagar este item Fsical da compra pois o mesmo está sendo utilizado");
                return;
            }
            await _compraFiscalRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(CompraFiscal compra)
        {
            if (!ExecutarValidacao(new CompraFiscalValidation(), compra))
                return;

            await _compraFiscalRepository.AtualizarSemSalvar(compra);
        }

        public async Task<List<CompraFiscal>> ObterFiscaisPorCompra(long id)
        {
            return _compraFiscalRepository.Obter(x => x.IDCOMPRA == id).Result.ToList();
        }

        public async Task<CompraFiscal> ObterFiscalPorId(long id)
        {
            return _compraFiscalRepository.ObterPorId(id).Result;
        }

        #endregion

        #region Dapper

        public async Task<bool> ImportarCompraDeXmlNfe(NFeProc nfe, long idCompra)
        {

            var resultado = false;

            try
            {
                var compraExistente = await _compraDapperRepository.ExisteCompraPorChaveAcesso(nfe.ProtNFe.InfProt.chNFe);
                if (compraExistente != null)
                {
                    Notificar($"O arquivo selecionado já foi importado em uma outra compra. Nº da Compra: {compraExistente.CDCOMPRA}");
                    return false;
                }

                var compra = await _compraDapperRepository.ObterCompraPorId(idCompra);
                if (compra == null)
                {
                    Notificar($"Erro ao tentar localizar a compra que será atualizada");
                    return false;
                }

                var fornecedor = _fornecedorDapperRepository.ObterFornecedorPorCNPJ(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ).Result;
                if (fornecedor == null)
                {
                    var cep = await _enderecoDapperRepository.ObterEnderecoPorCep(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.CEP);

                    var endereco = new Endereco(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xLgr, null, nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xBairro,
                        cep != null && string.IsNullOrEmpty(cep.Numero.Trim()) ? cep.Numero : nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.CEP,
                        nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xMun, nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.UF,
                        nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xPais, cep != null ? cep.ibge : 0, null,
                        nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.nro);

                    fornecedor = await _fornecedorDapperRepository.AdicionarFornecedor(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome,
                                                                            string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant.Trim()) ? nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant : nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome,
                                                                            ETipoPessoa.J, nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ,
                                                                            string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE.Trim()) ? nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE : nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IEST,
                                                                            ETipoFiscal.Distribuicao, endereco);

                    if (!string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.infRespTec.fone))
                    {
                        var contato = await _contatoDapperRepository.AdicionarContato(ETipoContato.TelefoneComercial, nfe.NotaFiscalEletronica.InformacoesNFe.infRespTec.fone, string.Empty, fornecedor.Id);
                        if (contato != null) await _contatoDapperRepository.AdicionarContatoFornecedor(contato.Id, fornecedor.Id);
                    }

                    if (!string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.infRespTec.email))
                    {
                        var contato = await _contatoDapperRepository.AdicionarContato(ETipoContato.Email, nfe.NotaFiscalEletronica.InformacoesNFe.infRespTec.email, string.Empty, fornecedor.Id);
                        if (contato != null) await _contatoDapperRepository.AdicionarContatoFornecedor(contato.Id, fornecedor.Id);
                    }

                }

                var dataRecebimento = DateTime.Parse(nfe.ProtNFe.InfProt.dhRecbto);

                var cfop = Convert.ToInt32(!string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe.FirstOrDefault().Produto.CFOP) ? nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe.FirstOrDefault().Produto.CFOP : "0");

                compra.AtualizarImportacaoNfe(DateTime.Now, dataRecebimento, nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF, nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie.ToString(),
                    nfe.ProtNFe.InfProt.chNFe, ETipoCompravanteCompra.NFE, cfop, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorICMSRetido,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorBaseCalculoICMS, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorICMS,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorBaseCalculoCst, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorIcmsSub,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorTotalProduto, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorFrete,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorSeguro, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorDesconto,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorOutros, nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorIPI,
                    nfe.NotaFiscalEletronica.InformacoesNFe.TotalNFE.IcmsTotal.ValorTotal, nfe.NotaFiscalEletronica.InformacoesNFe.InformacaoAdicional.infCpl,
                    (int)ESimNao.Sim);

                await _compraDapperRepository.AtualizarCompra(compra);

                //apagar possiveis itens da compra
                await _compraDapperRepository.ApagarItensPorIdCompra(compra.Id);

                //incluir itens
                nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe.ForEach(async item => {

                    var produto = await _produtoDapperRepository.ObterProdutoPorCodigoEan(!string.IsNullOrEmpty(item.Produto.cEAN.Trim()) ? item.Produto.cEAN : "");
                    if (produto == null)
                        produto = await _produtoDapperRepository.ObterProdutoPorCompraAnterior(fornecedor.Id, item.Produto.cProd);

                    long relacao = 0;
                    double novoValorVenda = 0;
                    long? idProduto = null;
                    if (produto != null)
                    {
                        relacao = produto.NURELACAO.HasValue ? produto.NURELACAO.Value : 0;
                        novoValorVenda = produto.NUPRECO.HasValue ? produto.NUPRECO.Value : 0;
                        idProduto = produto.Id;
                    }

                    var CDCSTIPI = "";
                    var VLALIQIPI = 0;
                    var VLBSCALCIPI = 0;
                    var VLIPI = 0;
                    int cfop = item.Produto.CFOP != null ? Convert.ToInt32(item.Produto.CFOP) : 0;
                    var cdCstIcms = item.imposto.ICMS.ICMS10.CST != null ? item.imposto.ICMS.ICMS10.CST : 0;

                    var compraItem = new CompraItem(compra.Id, idProduto, item.Produto.xProd, item.Produto.cEAN, item.Produto.NCM, item.Produto.CEST, item.Produto.uCom,
                        item.Produto.qCom, relacao, item.Produto.vUnCom, item.Produto.vProd, cfop, 0, 0, 0, 0, cdCstIcms.ToString(), item.imposto.Pis.PISAliq.CST,
                        item.imposto.COFINS.COFINSAliq.CST, CDCSTIPI, item.imposto.Pis.PISAliq.vPIS, item.imposto.COFINS.COFINSAliq.pCOFINS, item.imposto.ICMS.ICMS10.pICMS, VLALIQIPI, VLBSCALCIPI,
                        item.imposto.COFINS.COFINSAliq.vBC, item.imposto.ICMS.ICMS10.vBC, VLBSCALCIPI, item.imposto.ICMS.ICMS00.vICMS, item.imposto.Pis.PISAliq.vPIS, item.imposto.COFINS.COFINSAliq.vCOFINS,
                        VLIPI, item.Produto.cProd, novoValorVenda);

                    await _compraDapperRepository.AdicionarItem(compraItem);

                });
                resultado = true;
            }
            catch(Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
          return resultado;       
        }

        public async Task<bool> ImportarArquivoNFE(long idCompra, string ArquivoXml)
        {
            var resultado = false;
           
            try
            {
                await _dapperRepository.BeginTransaction();

                MemoryStream xmlStream = new MemoryStream();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ArquivoXml);
                xmlDoc.Save(xmlStream);
                xmlStream.Flush();//Adjust this if you want read your data 
                xmlStream.Position = 0;

                var serializer = new XmlSerializer(typeof(NFeProc));
                var nFeProc = (NFeProc)serializer.Deserialize(xmlStream);

                resultado = await ImportarCompraDeXmlNfe(nFeProc, idCompra);

                if (resultado)
                {
                    resultado = _compraDapperRepository.AdicionarFiscal(idCompra, xmlDoc.OuterXml).Result;
                    if (!resultado)
                        Notificar("Erro ao tentar adicionar arquivo Xml em compra fiscal");
                }                   

                if (resultado)
                    await _dapperRepository.Commit();                
                else
                    await _dapperRepository.Rollback();
            }
            catch(Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }

            return resultado;
        }

        public async Task<NFeProc> ImportarArquivoXmlNFE(long idCompra, string ArquivoXml)
        {
            var resultado = false;
            NFeProc nFeProc = new NFeProc();
            try
            {
                await _dapperRepository.BeginTransaction();

                MemoryStream xmlStream = new MemoryStream();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ArquivoXml);
                xmlDoc.Save(xmlStream);
                xmlStream.Flush();//Adjust this if you want read your data 
                xmlStream.Position = 0;

                var serializer = new XmlSerializer(typeof(NFeProc));
                nFeProc = (NFeProc)serializer.Deserialize(xmlStream);

                resultado = await ImportarCompraDeXmlNfe(nFeProc, idCompra);

                if (resultado)
                {
                    resultado = _compraDapperRepository.AdicionarFiscal(idCompra, xmlDoc.OuterXml).Result != null;
                    if (!resultado)
                        Notificar("Erro ao tentar adicionar arquivo Xml em compra fiscal");
                }

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }

            return nFeProc;
        }

        public async Task<bool> EfetivarCompra(long idCompra, string usuarioNome)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var compra = await _compraDapperRepository.ObterCompraPorId(idCompra);
                if (compra == null)
                {
                    Notificar($"Erro ao tentar localizar a compra que será efetivada");
                    return false;
                }

                var CONTA_REALIZARCONTROLE = _utilDapperRepository.ConfigRetornaValor("CONTA_REALIZARCONTROLE", compra.IDEMPRESA).Result;
                var realizaControleContabil = CONTA_REALIZARCONTROLE == "1" ? true: false;
                long idContaEstoque = 0;
                if (realizaControleContabil)
                {
                 
                    Int64.TryParse(_utilDapperRepository.ConfigRetornaValor("CONTA_IDCONTAESTOQUE", compra.IDEMPRESA).Result,out idContaEstoque);
                    if (idContaEstoque <= 0)
                    {
                        Notificar("Não é possível realizar o lançamento por que a conta de estoque não foi configurada. Acesse o menu de configurações na aba CONTA e preencha a informação");
                        return false;
                    }
                }

                //Obter Itens da compra
                var itensCompras = _compraDapperRepository.ObterCompraItemPorIdCompra(idCompra).Result;

                if (itensCompras.Any())
                {
                    //percorrer cada item da compra
                    itensCompras.ForEach(async item => {

                        //Tenta pegar a relação do Item, se não estiver preenchido, pega o padrão do Produto
                        var relacaoProduto = item.NURELACAO.HasValue && item.NURELACAO > 0 ? item.NURELACAO.Value : (item.Produto != null && item.Produto.NURELACAO.HasValue ? item.Produto.NURELACAO.Value : 0);

                        var valorUnitarioVenda = (item.VLUNIT.HasValue ? item.VLUNIT.Value : 0) / relacaoProduto;

                        var quantidadeEntrada = (item.NUQTD.HasValue ? item.NUQTD.Value : 0) * relacaoProduto;

                        long idProduto = item.IDPRODUTO.HasValue ? item.IDPRODUTO.Value : 0;
                        //O Valor unitário para fins de cálculo de custo médio e de valor de última compra
                        //deve ser o valor referente a unidade de Venda e não à unidade de Compra
                        var novoValorMedioProduto = _produtoDapperRepository.AtualizarCustoMedio(idProduto, quantidadeEntrada, valorUnitarioVenda).Result;

                        //atualizar ultimo valor de compra
                        await _produtoDapperRepository.AtualizarUltimoValorCompra(idProduto, valorUnitarioVenda);

                        //Só atualiza o novo preço de venda se esse for maior que zero
                        if (item.VLNOVOPRECOVENDA > 0)
                            await _produtoDapperRepository.AtualizarPrecoVenda(idProduto, item.VLNOVOPRECOVENDA.Value);

                        var idEstoqueHST = await _estoqueDapperRepository.RealizaEntradaRetornaIdHistoricoGerado(item.IDESTOQUE.Value, idProduto, item.Id, usuarioNome, $"Entrada por efetivação da compra Nº {compra.CDCOMPRA}",
                            quantidadeEntrada);

                        if (idEstoqueHST > 0)
                        {
                            //Se conseguir realizar a entrada em estoque, devo verificar e cadastrar o código de Barras
                            if (!string.IsNullOrEmpty(item.CDEAN))
                            {
                                //Pesquiso por um produto cadastrado. Caso não exista, devo incluir o EAN no produto atual
                                var produto = await _produtoDapperRepository.ObterProdutoPorCodigoEan(item.CDEAN);
                                await _produtoDapperRepository.InsereProdutoCodigoBarra(idProduto, item.CDEAN);
                            }

                            //Se conseguir realizar a entrada em estoque, devo registrar os lançamentos no plano de contas
                            var descricao = $"Entrada em estoque do produto  {item.DSPRODUTO}, através da compra Nº {compra.CDCOMPRA} e Nota Fiscal Nº {compra.NUNF}.";

                            var idLancamento = await _planoContaDapperRepository.RealizarLancamento(idContaEstoque, compra.DTCOMPRA.Value, descricao, item.VLTOTAL.Value, ETipoContaLancacmento.Debito);

                            //Se o lançamento foi efetuado com sucesso, associo ele ao histórico de estoque
                            if (idLancamento > 0)
                            {
                                await _estoqueDapperRepository.AtaulizarLancamentoEstoqueHistorico(idLancamento, idEstoqueHST);
                            }
                            else
                            {
                                Notificar($"Não foi possível realizar o lançamento na conta de estoque do item {item.DSPRODUTO}");
                            }
                        }
                        else
                        {
                            Notificar($"Não foi possível dar entrada no estoque do produto {item.DSPRODUTO}. Verifique os dados e tente novamente");
                        }
                    });

                    if (!TemNotificacao())
                    {

                        //Realiza a atualização de Saldo das Contas
                        if (realizaControleContabil)
                        {
                            var idContaPai = await _planoContaDapperRepository.ObterContaPrimeiroNivel(idContaEstoque);
                            await _planoContaDapperRepository.AtualizarSaldoContaESubConta(idContaPai);
                        }

                        await _compraDapperRepository.AtualizarSituacaoCompra(idCompra, ESituacaoCompra.Efetivada);
                    }

                }
                else
                {
                    Notificar("Não é possível realizar a efetivação de compras, pois não foram encontrados itens associados a produtos");                    
                }       
                
                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                 
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar efetivar compra");
                }                 

            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }

        public async Task<bool> RealizarCadastroProdutoAutomatico(long idCompra)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();
                                
                var compra = await _compraDapperRepository.ObterCompraPorId(idCompra);
                if (compra == null)
                {
                    Notificar($"Erro ao tentar localizar a compra, cujo produtos que serão cadastrados automaticamente");
                    return false;
                }

                if (_compraDapperRepository.ObterQtdItensNaoAssociados(idCompra).Result == 0)
                {
                    Notificar($"Não existem itens de compra com produto não associado na compra Nº {compra.CDCOMPRA}");
                    return false;
                }

                if(compra.STCOMPRA != ESituacaoCompra.Aberta)
                {
                    Notificar($"O Cadastro Automático só é possível para compras com situação ABERTA.', 'Realizar Cadastro Automático");
                    return false;
                }

                //Obter Itens da compra
                var itensCompras = _compraDapperRepository.ObterItemCompraPorIdCompraParaCadastroAutomatico(idCompra).Result;

                itensCompras.ForEach(async item => {
                    var idProduto = await _produtoDapperRepository.InsereProdutoPendente(item.DSPRODUTO.Substring(0,49),item.SGUN,item.CDNCM,item.CDCEST,item.NURELACAO.Value,item.VLNOVOPRECOVENDA.Value,compra.IDEMPRESA.Value);
                                      
                    if (idProduto > 0)
                        await _compraDapperRepository.AtualizarCompraItemComIdProduto(idProduto, item.Id);                
                });
               
                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();

                else
                {
                    await _dapperRepository.Rollback();
                    Notificar("Erro ao tentar cadastrar produto automaticamente");
                }
            }
            catch (Exception ex)
            {
                await _dapperRepository.Rollback();
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);
            }
            return resultado;
        }

        #endregion
        #region private
        private async Task<bool> PodeApagarCompra(long id) => true;
        private async Task<bool> PodeApagarCompraItem(long id) => true;
        private async Task<bool> PodeApagarCompraFiscal(long id) => true;

        public async Task<bool> CancelarCompra(long idCompra, string usuarioNome)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();
                long idContaEstoque = 0;

                var compra = await _compraDapperRepository.ObterCompraPorId(idCompra);
                if (compra == null)
                {
                    Notificar($"Erro ao tentar localizar a compra que será cancelada");
                    return false;
                }
              

                //Se a compra estiver aberta, basta mudar o status para CANCELADA
                if (compra.STCOMPRA == ESituacaoCompra.Aberta)
                {
                    await _compraDapperRepository.AtualizarSituacaoCompra(idCompra, ESituacaoCompra.Cancelada);
                }
                else if (compra.STCOMPRA == ESituacaoCompra.Efetivada)
                {
                    //Se a compra estiver efetivada, necessário retirar de estoque e cancelar os lançamentos contábeis
                    var itensCompra =await _compraDapperRepository.ObterItemCompraEfetivada(idCompra);

                    if (itensCompra.Any())
                    {
                        itensCompra.ForEach(item => {

                            long idlanc = 0;
                            if (itensCompra.Any())
                            {
                                if (item.EstoquesHistoricos.Any())
                                {
                                    var estoqueHistorico = item.EstoquesHistoricos.FirstOrDefault();
                                    if (estoqueHistorico != null)
                                    {
                                        idContaEstoque = estoqueHistorico.PlanoContaLancamento.IDCONTA.HasValue ? estoqueHistorico.PlanoContaLancamento.IDCONTA.Value : 0;
                                        idlanc = estoqueHistorico.IDLANC.HasValue ? estoqueHistorico.IDLANC.Value : 0;

                                        if (!_estoqueDapperRepository.DesvincularHistoricoDoLancamento(estoqueHistorico.Id).Result)
                                        {
                                            Notificar($"Não foi possível desvincular o lançamento da conta contábil referente ao item {item.DSPRODUTO}");
                                        }

                                        if (!_planoContaDapperRepository.ExcluirLancamento(idlanc).Result)
                                        {
                                            Notificar($"Não foi possível excluir o lançamento da conta contábil referente ao item {item.DSPRODUTO}");
                                        }

                                        var descricao = $"Retirada do estoque devido ao cancelamento da compra Nº {compra.CDCOMPRA}";

                                        if (_estoqueDapperRepository.RealizaRetiradaRetornaIdHistoricoGerado(estoqueHistorico.IDESTOQUE.Value, item.IDPRODUTO.Value, item.Id, usuarioNome, descricao, estoqueHistorico.QTDHST.Value).Result <= 0)
                                            Notificar($"Não foi possível realizar a retirada do estoque referente ao item  {item.DSPRODUTO}");
                                    }
                                }
                            }
                        });
                        await _planoContaDapperRepository.AtualizarSaldoContaESubConta(idContaEstoque);

                        await _compraDapperRepository.AtualizarSituacaoCompra(idCompra, ESituacaoCompra.Cancelada);
                    }
                    else
                    {
                        Notificar("Erro ao tentar cancelar a compra");
                    }                   
                }

                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        public async Task<bool> AtualizarProdutoNoItemCompra(long idItem, long idCompra, long? idProduto, long? idEstoque, string SGUN, double? Quantidade, double? Relacao, double? ValorUnitario, double? ValorTotal, double? NovoPrecoVenda)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                if (!_compraDapperRepository.AtualizarProdutoNoItemCompra(idItem, idCompra, idProduto, idEstoque, SGUN, Quantidade, Relacao, ValorUnitario, ValorTotal, NovoPrecoVenda).Result)
                    Notificar("Erro ao tenta atualizar Produto no item de compra");

                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }


        #endregion





    }
}
