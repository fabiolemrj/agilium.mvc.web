using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;
using static Dapper.SqlMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace agilium.api.infra.Repository
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AgiliumContext db) : base(db)
        {
        }

        public async Task<Empresa> ObterCompletoTracking(long id)
        {
            return await Db.Empresas
                .Include(e => e.Endereco)
                .Include(e => e.ContatoEmpresas)
                    .ThenInclude(c => c.Contato)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
