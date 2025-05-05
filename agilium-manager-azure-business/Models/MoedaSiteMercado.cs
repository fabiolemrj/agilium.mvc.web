using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class MoedaSiteMercado: Entity
    {
        public virtual Moeda Moeda { get; private set; }
        public long? IDMOEDA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public long? IDEMPRESA { get; private set; }
        public ETipoMoedaSiteMercado? IDSM { get; private set; }
        public DateTime? DTHRCAD { get; private set; }
        public MoedaSiteMercado()
        {            
        }

        public MoedaSiteMercado(long? iDMOEDA, long? iDEMPRESA, ETipoMoedaSiteMercado? iDSM, DateTime? dTHRCAD)
        {
            IDMOEDA = iDMOEDA;
            IDEMPRESA = iDEMPRESA;
            IDSM = iDSM;
            DTHRCAD = dTHRCAD;
        }
    }
}
