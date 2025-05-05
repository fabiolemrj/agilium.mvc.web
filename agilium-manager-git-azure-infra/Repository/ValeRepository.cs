using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using agilium.api.infra.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace agilium.api.infra.Repository
{
    public class ValeRepository : Repository<Vale>, IValeRepository
    {
        public ValeRepository(AgiliumContext db) : base(db)
        {
        }
        public override async Task<Vale> GerarCodigoPorSql(string sql)
        {
            var objeto =  DbSet.FromSqlRaw<Vale>(sql).ToList().FirstOrDefault();
            return objeto;
        }
    }
}
