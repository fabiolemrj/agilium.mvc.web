using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository
{
    public class MotivoDevolucaoRepository : Repository<MotivoDevolucao>, IMotivoDevolucaoRepository
    {
        public MotivoDevolucaoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class DevolucaoRepository : Repository<Devolucao>, IDevolucaoRepository
    {
        public DevolucaoRepository(AgiliumContext db) : base(db)
        {
        }

        public async Task<Devolucao> ObterDevolucaoPorId(long id)
        {

            return await Task.Run(() => {
                var query = from b in Db.Devolucao.Where(x => x.Id == id)
                            join p in Db.MotivosDevolucao on b.IDMOTDEV equals p.Id into grouping
                            from p in grouping.DefaultIfEmpty()
                            join v in Db.Vendas on b.IDVENDA equals v.Id into groupingVenda
                            from v in groupingVenda.DefaultIfEmpty()
                            join c in Db.Caixa on v.IDCAIXA equals c.Id into groupingCaixa
                            from c in groupingCaixa.DefaultIfEmpty()
                            join l in Db.Clientes on b.IDCLIENTE equals l.Id into groupingCLiente
                            from l in groupingCLiente.DefaultIfEmpty()
                            select new { b, p, v, c, l };

                var item = query.FirstOrDefault();
                var devolucao = new Devolucao(item.b.IDEMPRESA, item.b.IDVENDA, item.b.IDCLIENTE, item.b.IDMOTDEV, item.b.IDVALE, item.b.CDDEV, item.b.DTHRDEV, item.b.VLTOTALDEV,
                                                item.b.DSOBSDEV, item.b.STDEV);
                devolucao.Id = item.b.Id;

                item.v.AddCaixa(item.c);
                devolucao.AddVenda(item.v);
                devolucao.AddCliente(item.l);
                devolucao.AddMotivoDev(item.p);

                return devolucao;
            });
          
        }

        public async Task<List<Devolucao>> ObterDevolucaoPorPaginacao(long idEmpresa, DateTime dtIni, DateTime dtFim, int page = 1, int pageSize = 15)
        {
            var query = from b in Db.Devolucao.Where(x=> x.IDEMPRESA == idEmpresa &&x.DTHRDEV >= dtIni && x.DTHRDEV <= dtFim)
                        join p in Db.MotivosDevolucao on b.IDMOTDEV equals p.Id into grouping
                          from p in grouping.DefaultIfEmpty()
                        join v in Db.Vendas on b.IDVENDA equals v.Id into groupingVenda
                          from v in groupingVenda.DefaultIfEmpty()
                        join c in Db.Caixa on v.IDCAIXA equals c.Id into groupingCaixa
                        from c in groupingCaixa.DefaultIfEmpty()
                        join l in Db.Clientes on b.IDCLIENTE equals l.Id into groupingCLiente
                        from l in groupingCLiente.DefaultIfEmpty()

                        select new { b, p, v, c,l };

            var lista = new List<Devolucao>();

            await query.ForEachAsync( item => {
               
                var devolucao = new Devolucao(item.b.IDEMPRESA, item.b.IDVENDA, item.b.IDCLIENTE, item.b.IDMOTDEV, item.b.IDVALE, item.b.CDDEV, item.b.DTHRDEV, item.b.VLTOTALDEV,
                 item.b.DSOBSDEV, item.b.STDEV);
                
                devolucao.Id = item.b.Id;
                
                item.v.AddCaixa(item.c);
                    devolucao.AddVenda(item.v);
                    devolucao.AddCliente(item.l);
                    devolucao.AddMotivoDev(item.p);

                    lista.Add(devolucao);
                });
           
            return lista;
        }
    }

    public class DevolucaoItemRepository : Repository<DevolucaoItem>, IDevolucaoItemRepository
    {
        public DevolucaoItemRepository(AgiliumContext db) : base(db)
        {
        }
    }
}
