using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class ConfigRepository : Repository<agilium.api.business.Models.Config>, IConfigRepository
    {
        public ConfigRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class ConfigImagemRepository : Repository<ConfigImagem>, IConfigImagemRepository
    {
        public ConfigImagemRepository(AgiliumContext db) : base(db)
        {
        }
    }
}
