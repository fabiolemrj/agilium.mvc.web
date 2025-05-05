using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels
{
    public class RefreshToken
    {
        public RefreshToken()
        {
            Id = Guid.NewGuid();
            Token = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(256)]
        public string Username { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
