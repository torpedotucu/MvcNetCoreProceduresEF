using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;
        public DoctoresController (RepositoryDoctores repo)
        {
            this.repo=repo;
        }
        public async Task<IActionResult> Index()
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["ESPECIALIDADES"]=especialidades;
            return View();
        }
        public async Task<IActionResult> DoctoresEspecialidad()
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewData["ESPECIALIDADES"]=especialidades;
            return View();
        }
    }
}
