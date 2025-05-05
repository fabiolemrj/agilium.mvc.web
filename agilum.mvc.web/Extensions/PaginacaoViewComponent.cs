using agilum.mvc.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace agilum.mvc.web.Extensions
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList modeloPaginado)
        {
            return View(modeloPaginado);
        }
    }
}
