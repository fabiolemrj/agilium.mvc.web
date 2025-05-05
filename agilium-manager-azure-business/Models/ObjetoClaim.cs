using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ObjetoClaim: Entity
    {
        public string ClaimType { get; private set; }
        
        public ObjetoClaim(string claimType)
        {            
            ClaimType = claimType;
        }

        public ObjetoClaim()
        {

        }
    }
}
