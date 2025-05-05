using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = GerarId();
        }

        public long Id { get; set; }

        public long GerarId()
        {
            Guid guid = Guid.NewGuid();

            byte[] _bytes = guid.ToByteArray();
            return BitConverter.ToInt64(_bytes, 0);
        }
    }
}
