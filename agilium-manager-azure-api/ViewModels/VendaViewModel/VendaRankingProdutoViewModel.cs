using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace agilium.api.manager.ViewModels.VendaViewModel
{
    public class VendaRankingProdutoViewModel
    {
        public double valor { get; set; }
        public string produto { get; set; }
    }

    public class VendaRankingProdutoIndexViewModel 
    {
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public double? Total { get; set; } = 0;
        public List<VendaRankingProdutoViewModel> Ranking { get; set; } = new List<VendaRankingProdutoViewModel>();
    }
}
