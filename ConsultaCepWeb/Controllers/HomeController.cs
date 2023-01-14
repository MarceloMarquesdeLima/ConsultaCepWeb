using ConsultaCepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConsultaCepWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Models.Cep cep)
        {
            if (!ModelState.IsValid)
            {
                return View(cep);
            }

            var correios = new WSCorreios.AtendeClienteClient();

            var consulta = correios.consultaCEPAsync(cep.Codigo.Replace("-", "")).Result;

            if (consulta != null)
            {
                ViewBag.Endereco = new Models.Endereco()
                {
                    Descricao = consulta.@return.end,
                    Complemento = consulta.@return.complemento2,
                    Bairro = consulta.@return.bairro,
                    Cidade = consulta.@return.cidade,
                    UF = consulta.@return.uf
                };
            }

            return View(cep);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}