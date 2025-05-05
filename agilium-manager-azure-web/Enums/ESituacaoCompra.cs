namespace agilium.webapp.manager.mvc.Enums
{
    public enum ESituacaoCompra
    {
        Aberta = 1,
        Efetivada = 2,
        Cancelada = 3
    }

    public enum ETipoCompravanteCompra
    {
        NFE = 1,
        NFCE = 2,
        NFSE = 3,
        CupomFiscal = 4,
        Recibo = 5,
        NotaFiscalPapel = 6,
        Outros = 7
    }

    public enum ETipoManifestoCompra
    {
        NaoManifestada = 0,
        ManifestadaComAceite = 1,
        ManifestadaSemAceito = 2
    }
}
