using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class ObjetoClaimRepository : Repository<ObjetoClaim>, IClaimRepository
    {
        public ObjetoClaimRepository(AgiliumContext context) : base(context)
        {
        }
    }
}
