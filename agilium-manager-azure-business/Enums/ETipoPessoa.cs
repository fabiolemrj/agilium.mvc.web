using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace agilium.api.business.Enums
{
    public enum ETipoPessoa
    {
        [Display(Name = "Física")]
        F,
        [Display(Name = "Júridica")]
        J,
    }
}
