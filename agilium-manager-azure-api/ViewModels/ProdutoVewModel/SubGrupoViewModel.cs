using agilium.api.business.Enums;

namespace agilium.api.manager.ViewModels.ProdutoVewModel
{
    public class SubGrupoViewModel
    {        
        public long Id { get; set; }
        public long? IDGRUPO { get; set; }
        public string Nome { get; set; }
        public EAtivo? Situacao { get; set; }
        public string NomeGrupo { get; set; }
    }
}
