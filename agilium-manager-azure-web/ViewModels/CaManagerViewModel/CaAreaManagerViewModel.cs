using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.ViewModels.CaManagerViewModel
{
    public class CaAreaManagerViewModel
    {
        public int id { get; set; }
        public int IdArea { get; set; }
        public int hierarquia { get; set; }
        public int ordem { get; set; }
        public string titulo { get; set; }
        public int subitem { get; set; }
        public int idtag { get; set; }
        public bool Selecao { get; set; }
        public string text { get; set; }
        public bool @checked { get; set; }
        public List<CaAreaManagerViewModel> children { get; set; } = new List<CaAreaManagerViewModel>();
    }

    public class CaAreaManagerSalvarViewModel
    {
        public int IdPerfil { get; set; }
        public int IdArea { get; set; }
    }

    public class CaPerfilManagerViewModel
    {

        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
    }

    public class CaPermissaoPerfilViewModel
    {
        public CaPerfilManagerViewModel Perfil { get; set; } = new CaPerfilManagerViewModel();
        public List<CaAreaManagerViewModel> Area { get; set; } = new List<CaAreaManagerViewModel>();

    }

    public class LocationBd
    {
        public int id { get; set; }

        public string text { get; set; }

        public long? population { get; set; }

        public string flagUrl { get; set; }

        public bool @checked { get; set; }

        public bool hasChildren { get; set; }

        public virtual List<LocationBd> children { get; set; }
    }

    public class Location
    {
        public Location()
        {
            
        }

        public Location(int iD, int? parentID, string name, bool @checked, int orderNumber, long? population, string flagUrl, Location parent, List<Location> children)
        {
            ID = iD;
            ParentID = parentID;
            Name = name;
            Checked = @checked;
            OrderNumber = orderNumber;
            Population = population;
            FlagUrl = flagUrl;
            Parent = parent;
            Children = children;
        }

        public int ID { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public int OrderNumber { get; set; }

        public long? Population { get; set; }

        public string FlagUrl { get; set; }

        public virtual Location Parent { get; set; }

        public virtual List<Location> Children { get; set; }
    }
}