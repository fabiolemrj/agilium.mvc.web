using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoItemRepository : Repository<PedidoItem>, IPedidoItemRepository
    {
        public PedidoItemRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoSiteMercadoRepository : Repository<PedidoSitemercado>, IPedidoSiteMercadoRepository
    {
        public PedidoSiteMercadoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoItemSiteMercadoRepository : Repository<PedidoItemSitemercado>, IPedidoSiteMercadoItemRepository
    {
        public PedidoItemSiteMercadoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoPagamentoRepository : Repository<PedidoPagamento>, IPedidoPagamentoRepository
    {
        public PedidoPagamentoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoPagamentoSiteMercadoRepository : Repository<PedidoPagamentoSitemercado>, IPedidoPagamentoSiteMercadoRepository
    {
        public PedidoPagamentoSiteMercadoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class PedidoVendaRepository : Repository<PedidoVenda>, IPedidoVendaRepository
    {
        public PedidoVendaRepository(AgiliumContext db) : base(db)
        {
        }
    }
    public class PedidoVendaItemRepository : Repository<PedidoVendaItem>, IPedidoVendaItemRepository
    {
        public PedidoVendaItemRepository(AgiliumContext db) : base(db)
        {
        }
    }
}
