using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels
{
    public class UserClaimViewModel
    {
        public string Id { get; set; }
        [StringLength(10)]
        [Required]
        public string claimType { get; set; }
    }

    public class DuplicarClaimUsuarioViewModel
    {
        public string idOrigem { get; set; }
        public string idDestino { get; set; }
    }
}
