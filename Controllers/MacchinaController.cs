using Microsoft.AspNetCore.Mvc;
using project_veicoli_ado.Models;
using project_veicoli_ado.Services.Interface;
using project_veicoli_ado.ViewModels;

namespace project_veicoli_ado.Controllers
{
    public class MacchinaController : Controller
    {

        private readonly IVeicoliService<MacchinaViewModel> _veicoliService;

        public MacchinaController(IVeicoliService<MacchinaViewModel> veicoliService)
        {
            _veicoliService = veicoliService;
        }

        public IActionResult Index()
        {
            List<MacchinaViewModel> macchine = _veicoliService.GetAll();
            return View(macchine);
        }
    }
}
