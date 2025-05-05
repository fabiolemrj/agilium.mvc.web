using System.ComponentModel.DataAnnotations;
using System;

namespace agilium.api.pdv.ViewModels
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
