using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class CaModeloRepository : Repository<CaModelo>, ICaModeloRepository
    {
        public CaModeloRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CaPerfilRepository : Repository<CaPerfil>, ICaPerfilRepository
    {
        public CaPerfilRepository(AgiliumContext db) : base(db)
        {
        }
    }
    public class CaPermissaoItemRepository : Repository<CaPermissaoItem>, ICaPermissaoItemRepository
    {
        public CaPermissaoItemRepository(AgiliumContext db) : base(db)
        {
        }
    }

    #region CaManager
    public class CaPermissaoManagerRepository : Repository<CaPermissaoManager>, ICaPermissaoManagerRepository
    {
        public CaPermissaoManagerRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CaPerfilManagerRepository : Repository<CaPerfiManager>, ICaPerfilManagerRepository
    {
        public CaPerfilManagerRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CaAreaManagerRepository : Repository<CaAreaManager>, ICaAreaManagerRepository
    {
        public CaAreaManagerRepository(AgiliumContext db) : base(db)
        {
        }
    }
    #endregion
}
