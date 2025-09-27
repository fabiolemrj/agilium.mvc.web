using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository.Dapper
{
    public class LicencaDapperRespository : ILicencaDapperRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;
        private readonly IUtilDapperRepository _utilDapperRepository;

        public LicencaDapperRespository(IConfiguration configuration, DbSession dbSession, IUtilDapperRepository utilDapperRepository)
        {
            _configuration = configuration;
            _dbSession = dbSession;
            _utilDapperRepository = utilDapperRepository;
        }

    }
}
