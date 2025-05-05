using System.ComponentModel;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ESituacaoNFInutil
    {
        [Description("Aguardando Envio ao Sefaz")]
        AguardandoEnvio = 1,
        [Description("Envido ao Sefaz")]
        EnviadoSefaz = 2
    }
}
