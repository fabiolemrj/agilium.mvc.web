﻿using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(AgiliumContext db) : base(db)
        {
        }
    }

    public class CepRepository : Repository<Cep>, ICepRepository
    {
        public CepRepository(AgiliumContext db) : base(db)
        {
        }
    }
}
