using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ClaimValue:Entity
    {
        public long idClaimValue { get; private set; }
        public string Value { get; private set; }
        public ClaimValue()
        {

        }

        public ClaimValue( string value)
        {
            Value = value;
        }

        public ClaimValue(long idClaimValue, string value)
        {
            this.idClaimValue = idClaimValue;
            Value = value;
        }

        
    }
}
