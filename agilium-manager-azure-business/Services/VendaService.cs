using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ComprasNFEViewModel;
using agilium.api.business.Models.CustomReturn.ReportViewModel.VendaReportViewModel;
using agilium.api.business.Models.CustomReturn.VendaCustomViewModel;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class VendaService : BaseService, IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IVendaItemRepository _vendaItemRepository;
        private readonly IVendaMoedaRepository _vendaMoedaRepository;
        private readonly IVendaEspelhoRepository _vendaEspelhoRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IVendaDapperRepository _vendaDapperRepository;
        private readonly IConfigDapperRepository _configDapperRepository;
        private readonly ICaixaDapperRepository _caixaDapperRepository;
        private readonly IPTurnoDapperRepository _turnoDapperRepository;
        private readonly IPedidoDapperRepository _pedidoDapperRepository;
        private readonly IValeDapperRepository _valeDapperRepository;


        public VendaService(INotificador notificador,IVendaRepository vendaRepository, IVendaMoedaRepository vendaMoedaRepository,
            IVendaItemRepository vendaItemRepository, IVendaEspelhoRepository vendaEspelhoRepository,
            IDapperRepository dapperRepository, IVendaDapperRepository vendaDapperRepository
            , IConfigDapperRepository configDapperRepository, ICaixaDapperRepository caixaDapperRepository, IPTurnoDapperRepository turnoDapperRepository,
            IPedidoDapperRepository pedidoDapperRepository, IValeDapperRepository valeDapperRepository) : base(notificador)
        {
            _vendaRepository = vendaRepository;
            _vendaItemRepository = vendaItemRepository;
            _vendaMoedaRepository = vendaMoedaRepository;
            _vendaEspelhoRepository = vendaEspelhoRepository;
            _dapperRepository = dapperRepository;
            _vendaDapperRepository = vendaDapperRepository;
            _configDapperRepository = configDapperRepository;
            _caixaDapperRepository = caixaDapperRepository;
            _turnoDapperRepository = turnoDapperRepository;
            _pedidoDapperRepository = pedidoDapperRepository;
            _valeDapperRepository = valeDapperRepository;
        }

        public async Task Salvar()
        {
            await _vendaRepository.SaveChanges() ;
        }

        public void Dispose()
        {          
            _vendaEspelhoRepository?.Dispose();
            _vendaMoedaRepository?.Dispose();
            _vendaItemRepository?.Dispose();
            _vendaRepository?.Dispose();
        }

        #region Venda
        public async Task Adicionar(Venda venda)
        {
            if (!ExecutarValidacao(new VendaValidation(), venda))
                return;

            await _vendaRepository.AdicionarSemSalvar(venda);
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagarVenda(id).Result)
            {
                Notificar("Não é possivel apagar esta venda pois o mesmo está sendo utilizado");
                return;
            }
            await _vendaRepository.RemoverSemSalvar(id);
        }

        public async Task Atualizar(Venda venda)
        {
            if (!ExecutarValidacao(new VendaValidation(), venda))
                return;

            await _vendaRepository.AtualizarSemSalvar(venda);
        }

        public async Task<Venda> ObterCompletoPorId(long id)
        {
            return _vendaRepository.Obter(x=>x.Id == id, "Caixa", "Caixa.PontoVenda", "Caixa.Funcionario", "Cliente").Result.FirstOrDefault();
        }
        
        public async Task<List<Venda>> ObterPorDescricao(string descricao)
        {
            return _vendaRepository.Obter(x=>x.SQVENDA.ToString() == descricao).Result.ToList();
        }

        public async Task<Venda> ObterPorId(long id)
        {
            return await _vendaRepository.ObterPorId(id);
        }

        public async Task<PagedResult<Venda>> ObterPorPaginacao(DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var lista = _vendaRepository.Obter(x => x.DTHRVENDA.Value >= dtIni && x.DTHRVENDA <= dtFim, "Caixa", "Caixa.PontoVenda", "Caixa.Funcionario", "Cliente").Result;

            return new PagedResult<Venda>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<List<Venda>> ObterTodas()
        {
            return await _vendaRepository.ObterTodos();
        }

        public async Task<double> ObterValorTotalVenda(long id)
        {
            var venda = _vendaRepository.ObterPorId(id).Result;
            if(venda != null)
                return (venda.VLVENDA.HasValue ? venda.VLVENDA.Value : 0) + (venda.VLACRES.HasValue ? venda.VLACRES.Value : 0) - (venda.VLDESC.HasValue? venda.VLDESC.Value : 0);
            return 0;
        }


        public async Task<List<Venda>> ObterVendaPorData(DateTime dtIni, DateTime dtFim)
        {

            var lista = _vendaRepository.Obter(x => x.DTHRVENDA.Value >= dtIni && x.DTHRVENDA <= dtFim, "Caixa", "Caixa.PontoVenda", "Caixa.Funcionario", "Cliente").Result;
            return lista.ToList();
        }
        #endregion

        #region Venda Item

        public async Task<List<VendaItem>> ObterItensVenda(long idVenda)
        {
            
            return _vendaItemRepository.Obter(x=>x.IDVENDA == idVenda,"Produto").Result.ToList();
        }

        public async Task<VendaItem> ObterItemPorVenda(long idItemVenda)
        {
            return await _vendaItemRepository.ObterPorId(idItemVenda);
        }

        #endregion

        #region Venda Moeda
        public async Task<List<VendaMoeda>> ObterMoedasVenda(long idVenda)
        {
            return _vendaMoedaRepository.Obter(x => x.IDVENDA == idVenda,"Moeda").Result.ToList();
        }
        #endregion

        #region Venda Espelho
        public async Task<VendaEspelho> ObterVendaEspelhoPorIdVenda(long idVenda)
        {
            return _vendaEspelhoRepository.Obter(x=>x.IDVENDA == idVenda).Result.FirstOrDefault();
        }

        #endregion

        #region Report

        public async Task<VendasReportViewModel> ObterRelatorioVendaDetalhada(DateTime dataInicial, DateTime dataFinal)
        {
            var vendas = new VendasReportViewModel();
            try
            {
                await _dapperRepository.BeginTransaction();

                var vendasReport = await _vendaDapperRepository.ObterVendasReportViewModel(dataInicial, dataFinal);
                
                double totalAcrescimo = 0;
                double totalDesconto = 0;
                double totalDevolucao = 0;
                double total = 0;
                double subTotal = 0;
                double quantidade = 0;

                vendasReport.ForEach(async venda => {
                    var vendasDetalhe = new VendaDetalheReportViewModel() { 
                        Acrescimo = venda.Acrescimo,
                        Desconto = venda.Desconto,
                        Devolucao = venda.Devolucao,
                        Id = venda.Id,
                        Operador = venda.Operador,
                        SeqCaixa = venda.SeqCaixa,
                        Situacao = venda.Situacao,
                        Pdv = venda.Pdv,
                        Total = venda.Total,
                        Valor = venda.Valor,
                        Sequencial = venda.Sequencial,
                        DataVenda = venda.DataVenda,
                    };

                    vendasDetalhe.Itens = await _vendaDapperRepository.ObterItensVendaReportViewModelPorVenda(venda.Id);
                    vendasDetalhe.Moedas = await _vendaDapperRepository.ObterMoedaItensVendaReportViewModelPorVenda(venda.Id);                   

                    subTotal += venda.Total;
                    totalAcrescimo += venda.Acrescimo;
                    totalDesconto += venda.Desconto;
                    totalDevolucao += venda.Devolucao;
                    total += (venda.Total + venda.Acrescimo - (venda.Desconto - venda.Devolucao));
                    quantidade += vendasDetalhe.Itens.Sum(x => x.Quantidade);
                    
                    vendas.Vendas.Add(vendasDetalhe);
                });
                vendas.TotalMoedas = await _vendaDapperRepository.ObterMoedaTotalReportViewModelPorVenda(dataInicial, dataFinal);
                vendas.SubTotal = subTotal;
                vendas.ValorTotal = total;
                vendas.TotalAcrescimo = totalAcrescimo;
                vendas.TotalDevolucao = totalDevolucao;
                vendas.TotalDesconto = totalDesconto;
                vendas.TotalQuantidade = quantidade;

                if (!TemNotificacao())
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
            return vendas;
        }


        private async Task<string> ObterConfig(string chave, long idEmpresa)
        {
            var config = await _configDapperRepository.ObterConfig(chave, idEmpresa);

            if (config != null && !string.IsNullOrEmpty(config.VALOR))
            {
                return config.VALOR;
            }

            return string.Empty;
        }

        private async Task<int> ObterConfigInteiro(string chave, long idEmpresa)
        {
            var config = await _configDapperRepository.ObterConfig(chave, idEmpresa);

            var result = 0;
            if (config != null && !string.IsNullOrEmpty(config.VALOR))
            {
                try
                {
                    result = Convert.ToInt32(config.VALOR);
                }
                catch
                {

                    result = 0;
                }
                
            }

            return result;
        }

        public async Task<bool> EmitirNFCe(long idVenda,long idEmpresa, long idUsuario,bool utilizeTabelaTemporaria, bool homologacao)
        {
            var resultado = false;
            var numNF = await _vendaDapperRepository.GerarNuNf(idEmpresa, homologacao);
            if (numNF <= 0) 
            {
                Notificar("Não é possível emitir a NFCe. Número da Nota Fiscal não preenchido");
                return resultado;
            }

            var nfe = new NFeProc();

            var empresa = await _vendaDapperRepository.ObterEmpresaPorId(idEmpresa);
            if(empresa == null)  
            {
                Notificar("Não foi possivel localizar a empresa que está vinculada da venda");
                return resultado;
            }

            var idFuncionario = await _caixaDapperRepository.ObterIdFuncionarioPorUsuarioEmpresa(idEmpresa, idUsuario);
            if (idFuncionario == 0)
            {
                Notificar($"Não foi localizado cadastro de funcionário.");
            }

            var caixaAberto = await _caixaDapperRepository.ObterCaixaAberto(idEmpresa, idFuncionario);
            if (caixaAberto == null)
            {
                Notificar("Não existe caixa aberto para o usuario.");
                return resultado;
            }

            var pdv = await _vendaDapperRepository.ObterPdvPorId(caixaAberto.IDPDV.Value);
            if (pdv == null)
            {
                Notificar("Não foi localizado Ponto de Venda a venda.");
                return resultado;
            }

            var endereco = await _vendaDapperRepository.ObterEnderecoPorId(empresa.IDENDERECO);
            if (endereco == null)
            {
                Notificar("Não foi localizado endereço da empresa para emissão de NFE");
                return resultado;
            }

            var venda = await _vendaDapperRepository.ObterVendaPorId(idVenda);
            if(venda == null)
            {
                Notificar("Não foi localizada venda para emissão de NFE");
                return resultado;
            }

            var cliente = await _vendaDapperRepository.ObterClientePorId(venda.IDCLIENTE.HasValue ? venda.IDCLIENTE.Value : 0);

            //identificação
            if (!homologacao)
            {
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.mod = await ObterConfig("NFCE_MODELO",idEmpresa);// 65;//55;
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie = await ObterConfigInteiro("NFCE_SERIE", idEmpresa);
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.natOp = await ObterConfig("NFCE_NATOP", idEmpresa);//'VENDA DE MERCADORIA';
            }
            else
            {
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.mod = await ObterConfig("NFCE_MODELO_HOMOL", idEmpresa);// 65;//55;
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie = await ObterConfigInteiro("NFCE_SERIE_HOMOL", idEmpresa);
                nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.natOp = await ObterConfig("NFCE_NATOP_HOMOL", idEmpresa);//'VENDA DE MERCADORIA';
            }
            var dataAtual = DateTime.Now;
            nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cNF = (1000000 + numNF).ToString();//Regra de Validação B03-10
            nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF = numNF.ToString();
            nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.dhEmi = dataAtual;
            //nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cUF = UFtoCUF(endereco.Uf);

            //emitente
            //nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CRT = StrToCRT(ConverteOk, empresa.CRT);
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome = empresa.NMRZSOCIAL;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant = empresa.NMFANTASIA;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ = empresa.NUCNPJ;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE = empresa.DSINSCREST;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IEST = "";
            //nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.fone = 
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xLgr = endereco.Logradouro;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.nro = endereco.Numero;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xBairro = endereco.Bairro;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.cMun = endereco.Ibge.HasValue? endereco.Ibge.Value.ToString() : "";
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.CEP = endereco.Cep;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.cPais = 1058;
            nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xPais = "BRASIL";

            // informações do destinatário da nota fiscal
            if (!string.IsNullOrEmpty(venda.NUCPFCNPJ))
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.CNPJ = venda.NUCPFCNPJ;

            if (cliente != null)
            {
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xLgr = cliente.Endereco.Logradouro;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.nro = cliente.Endereco.Numero;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xBairro = cliente.Endereco.Bairro;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xMun = cliente.Endereco.Cidade;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.cMun = endereco.Ibge.HasValue ? endereco.Ibge.Value.ToString() : "";
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.UF = endereco.Uf;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.CEP = endereco.Cep;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.cPais = 1058;
                nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xPais = "Brasil";
            }

            return resultado;
        }

        #endregion

        #region private
        private async Task<bool>PodeApagarVenda(long id)
        {
            var resultado = true;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        public async Task<VendasFornecedorViewModel> ObterRelatorioVendaPorFornecedor(DateTime dataInicial, DateTime dataFinal)
        {
            var resultado = new VendasFornecedorViewModel();
            try
            {
                await _dapperRepository.BeginTransaction();

                var vendasReport = await _vendaDapperRepository.ObterVendasPorFornecedor(dataInicial, dataFinal);
                resultado.Vendas = vendasReport;
                resultado.TotalQuantidade = vendasReport.Sum(x => x.Quantidade);
                resultado.TotalValor = vendasReport.Sum(x => x.Total);

                if (!TemNotificacao())
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

        public async Task<VendaMoedaReport> ObterRelatorioVendaPorMoeda(DateTime dataInicial, DateTime dataFinal)
        {
            var resultado = new VendaMoedaReport();
            try
            {
                await _dapperRepository.BeginTransaction();

                var dataVendasAgrupadas = await _vendaDapperRepository.ObterListaVendaAgrupadasPorData(dataInicial, dataFinal);
                
                dataVendasAgrupadas.ForEach(async data => {
                    var listaDatasVendaReport = new ListaDatasVendaReport();
                    listaDatasVendaReport.DataVenda = data;
                    listaDatasVendaReport.TotalVendaMoedaPorDataReport = await _vendaDapperRepository.ObterVendaMoedaTotalizadasPorData(data);

                    resultado.ListaDatasVendaReports.Add(listaDatasVendaReport);                
                });

                resultado.TotalizacaoMoeda = await _vendaDapperRepository.ObterVendaMoedaTotalizadasPorData(dataInicial,dataFinal);
                
                if (!TemNotificacao())
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

        public async Task<List<VendaRankingReport>> ObterVendaRankingPorData(DateTime dataInicial, DateTime dataFinal, EResultadoFiltroRanking tipoResultado, EOrdenacaoFiltroRanking ordenacao)
        {
            var resultado = new List<VendaRankingReport>();
            try
            {
                await _dapperRepository.BeginTransaction();

                resultado = await _vendaDapperRepository.ObterVendaRankingPorData(dataInicial, dataFinal,tipoResultado,ordenacao);

                if (!TemNotificacao())
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

        public async Task<List<VendaDiferencaCaixaReport>> ObterVendaDiferencaCaixa(DateTime dataInicial, DateTime dataFinal)
        {
            var resultado = new List<VendaDiferencaCaixaReport>();
            try
            {
                await _dapperRepository.BeginTransaction();

                var lista = await _vendaDapperRepository.ObterVendaDiferencaCaixa(dataInicial, dataFinal);

                lista.ForEach(item => { 
                    if(item.VLFECH != 0)
                    {
                        item.Valor = item.VLFECH > 0 ? item.VLFECH : item.VLFECH * -1;
                        item.Classificacao = item.VLFECH > 0 ? 1 : 2;

                        resultado.Add(item);
                    }
                });
                if (!TemNotificacao())
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

        public async Task<VendaNovaCustomViewModel> ObterDadosParaNovaVenda(long idUsuario, long idEmpresa)
        {
            var resultado = new VendaNovaCustomViewModel();

            var empresa = await _vendaDapperRepository.ObterEmpresaPorId(idEmpresa);
            if (empresa == null)
            {
                Notificar("Não foi possivel localizar a empresa que está vinculada da venda");
                return resultado;
            }

            var idFuncionario = await _caixaDapperRepository.ObterIdFuncionarioPorUsuarioEmpresa(idEmpresa, idUsuario);
            if (idFuncionario == 0)
            {
                Notificar($"Não foi localizado cadastro de funcionário.");
            }

            var caixaAberto = await _caixaDapperRepository.ObterCaixaAberto(idEmpresa, idFuncionario);
            if (caixaAberto == null)
            {
                Notificar("Não existe caixa aberto para o usuario.");
                return resultado;
            }

            var pdv = await _vendaDapperRepository.ObterPdvPorId(caixaAberto.IDPDV.Value);
            if (pdv == null)
            {
                Notificar("Não foi localizado Ponto de Venda a venda.");
                return resultado;
            }

            var turno = await _turnoDapperRepository.ObterTurnoAberto(idEmpresa);

            resultado.IdCaixa = caixaAberto.Id.ToString();
            resultado.SqCaixa = caixaAberto.SQCAIXA.HasValue?caixaAberto.SQCAIXA.Value:0;
            resultado.IdPdv = caixaAberto.IDPDV.ToString();
            if (turno != null && turno.NUTURNO.HasValue)
                resultado.Turno = turno.NUTURNO.Value;

            resultado.SqVenda = await _vendaDapperRepository.GerarSqVendaPorCaixa(caixaAberto.Id);
            resultado.IdEstoque = _caixaDapperRepository.ObterEstoquePorIdCaixa(caixaAberto.Id).Result.ToString();

            return resultado;
        }

        #endregion

        #region Dapper

        public async Task<bool> RealizarVenda(Venda venda, long idUsusario, long IdEmpresa)
        {
            var resultado = false;

            if (!ExecutarValidacao(new VendaValidation(), venda))
                return resultado;

            if (!venda.VendaItem.Any())
            {
                Notificar("Não existem itens da venda.");
                return resultado;
            }

            if (!venda.VendaMoeda.Any())
            {
                Notificar("Não existem formas de pagamento para venda.");
                return resultado;
            }

            if(venda.VLVENDA <= 0 || venda.VLTOTAL <= 0)
            {
                Notificar("Erro no valor da venda");
                return resultado;
            }

            foreach(var item in venda.VendaItem)
            {
                if (item.VLTOTAL <= 0 || item.VLITEM <= 0)
                {
                    Notificar("Erro no valores de itens da venda");
                    break;
                }

                if(item.NUQTD <= 0)
                {
                    Notificar("Erro na quantidade de itens da venda");
                    break;
                }
            }

            foreach(var moeda in venda.VendaMoeda)
            {
                if(moeda.VLPAGO <= 0)
                {
                    Notificar("Erro no valores de pagamento da venda");
                    break;
                }
            }

            if (TemNotificacao()) return resultado;

            var usuario = await _pedidoDapperRepository.ObterUsuarioPorId(idUsusario);
            if (usuario == null)
            {
                Notificar("Usuario do sistema não localizado.");
                return resultado;
            }

            var idFuncionario = await _caixaDapperRepository.ObterIdFuncionarioPorUsuarioEmpresa(IdEmpresa, idUsusario);
            if (idFuncionario == 0)
            {
                Notificar($"Não foi localizado cadastro de funcionário.");
            }

            var caixaAberto = await _caixaDapperRepository.ObterCaixaAberto(IdEmpresa, idFuncionario);
            if (caixaAberto == null)
            {
                Notificar("Não existe caixa aberto para o usuario.");
                return resultado;
            }

            var idestoque = await _caixaDapperRepository.ObterEstoquePorIdCaixa(caixaAberto.Id);
            if (idestoque == 0)
            {
                Notificar("Erro ao tentar localziar o estoque do produto");
                return resultado;
            }
            
            try
            {
                await _dapperRepository.BeginTransaction();

                var seqVenda = await _pedidoDapperRepository.GerarCodigoVenda(caixaAberto.Id);
                var cpf = await _pedidoDapperRepository.ObterCpfCnpjPorCliente(venda.IDCLIENTE.HasValue ?venda.IDCLIENTE.Value:0);
              
                var config = await _configDapperRepository.ObterConfig("VENDAS_DOC_FISCAL_PADRAO", IdEmpresa);
                var tpdoc = ETipoDocVenda.NFCE;

                var dsinfcompl = $@"Venda referente ao pedido ";
                if (config != null && !string.IsNullOrEmpty(config.VALOR))
                {
                    tpdoc = (ETipoDocVenda)Enum.Parse(typeof(ETipoDocVenda), config.VALOR);
                }

                venda.AdicionarOrigemVenda(EOrigemVenda.DIRETA);
                venda.MudarSituacaoAtivo();
                
                double? PCIBPTFED = 0d;
                double? PCIBPTEST = 0d;
                double? PCIBPTIMP = 0d;
                double? PCIBPTMUN = 0d;

                venda.AdicionarIbpt(PCIBPTFED, PCIBPTEST, PCIBPTMUN, PCIBPTIMP);

                venda.AdicionarInformacaoComplementar("Val aprox Tributos");
                venda.AdicionarInformacaoComplementar($"Federal R$ {(venda.VLTOTIBPTFED + venda.VLTOTIBPTIMP)?.ToString("C")} ({(((venda.VLTOTIBPTFED + venda.VLTOTIBPTIMP) * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Estadual R$ {venda.VLTOTIBPTEST?.ToString("C")} ({((venda.VLTOTIBPTEST * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Municipal R$ {venda.VLTOTIBPTMUN?.ToString("C")} ({((venda.VLTOTIBPTMUN * 100) / venda.VLVENDA)?.ToString("C")} %)");
                venda.AdicionarInformacaoComplementar($"Fonte: IBPT {dsinfcompl}");                
             
                var configPreVenda = await _configDapperRepository.ObterConfig("PDV_PREVENDA", IdEmpresa);

                var mEISemCupom = false;
                if (configPreVenda != null && !string.IsNullOrEmpty(configPreVenda.VALOR))
                {
                    mEISemCupom = configPreVenda.VALOR == "S";
                }

                var configPreVendaAtivo = await _configDapperRepository.ObterConfig("PREVENDA_ATIVO", IdEmpresa);

                var preVendaAtivo = false;
                if (configPreVendaAtivo != null && !string.IsNullOrEmpty(configPreVenda.VALOR))
                {
                    preVendaAtivo = configPreVendaAtivo.VALOR == "S";
                }

                var idVendatemp = await _vendaDapperRepository.AdicionarVendaTemporaria(venda, idestoque, caixaAberto.SQCAIXA.Value, usuario.nome, cpf);

                if (!preVendaAtivo && !mEISemCupom)
                {

                }
                
                if(idVendatemp > 0)
                {                 
                    var idVenda = await _vendaDapperRepository.AdicionarVenda(venda, idestoque, caixaAberto.SQCAIXA.Value, usuario.nome, cpf);
                    await _vendaDapperRepository.ApagarVendaTemporaria(idVendatemp);

                    await _valeDapperRepository.UtilizarValePorVenda(idVenda);
                }
                               
                if (!TemNotificacao())
                {
                    resultado = true;
                    await _dapperRepository.Commit();
                }
                else
                {
                    resultado = false;
                    await _dapperRepository.Rollback();
                }
            }
            catch
            {
                await _dapperRepository.Rollback();
                Notificar("Erro não identificado");
            }
            return resultado;
        }
        #endregion
    }
}
