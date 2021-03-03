using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab02.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Lab02.Extra;
using System.IO;
using ListaDobleEnlace;
using CsvHelper;
using System.Globalization;

namespace Lab02.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string option)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Prueba()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // Metodo de busqueda
        public IActionResult Busqueda()
        {
            Singleton.Instance.Indice.Preorden(Singleton.Instance.Indice.raiz, Singleton.Instance.ListaFarmacos);
            return View("Prueba");
        }

        // Método para leer archivo CSV

        [HttpGet]
        public IActionResult Index(ListaDoble<InfoFarmaco> ListaFarmacos = null)
        {
            if(ListaFarmacos.inicio != null)
            {
                Singleton.Instance.ListaFarmacos = ListaFarmacos;
                Singleton.Instance.Actualizar();
            }
            return View(Singleton.Instance.ListaFarmacos);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingenvironment)
        {
            string fileName = $"{hostingenvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            var Farmacos = this.GetFarmacosList(file.FileName);
            return Index(Farmacos);
        }

        private ListaDoble<InfoFarmaco> GetFarmacosList(string filename)
        {
            ListaDoble<InfoFarmaco> farmacos = new ListaDoble<InfoFarmaco>();

            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + filename;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var farmaco = csv.GetRecord<InfoFarmaco>();
                    farmacos.InsertarFinal(farmaco);
                }
            }

            path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\FlesTo"}";
            using (var write = new StreamWriter(path + "\\Archivo.csv"))
            using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(farmacos);
            }

            return farmacos;

        }
    }
}
