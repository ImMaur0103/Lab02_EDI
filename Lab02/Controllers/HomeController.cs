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
using Arbol;

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

        public IActionResult Buscar()
        {
            return View("Pedido");
        }
        
        public IActionResult Caja()
        {
            return View(Singleton.Instance.Compra);
        }


        public IActionResult Pedido(string nombre, string direccion, string nit, string cadena = "")
        {
            ListaDoble<InfoFarmaco> infoFarmacos = null;
            Singleton.Instance.Pedido = null;
            if (cadena != null)
            {
                cadena = cadena.ToLower();
                int posicion = Singleton.Instance.Indice.Buscar(cadena);
                if(posicion > 0)
                {
                    InfoFarmaco infoFarmaco = Singleton.Instance.ListaFarmacos.ObtenerValor(posicion - 1);
                    infoFarmacos = new ListaDoble<InfoFarmaco>();
                    infoFarmacos.InsertarInicio(infoFarmaco);
                    Singleton.Instance.Pedido = infoFarmacos;
                    cadena = "";
                    return View(Singleton.Instance.Pedido);
                }
            }
            return View();
        }

        public IActionResult Agregar()
        {
            if(Singleton.Instance.Pedido != null && Singleton.Instance.Compra.inicio == null)
            {
                InfoFarmaco Medicamento = new InfoFarmaco();
                InfoFarmaco info = Singleton.Instance.Pedido.ObtenerValor(0);
                Medicamento.Existencia = 1;
                Medicamento.Nombre = info.Nombre;
                Medicamento.ID = info.ID;
                Medicamento.Descripcion = info.Descripcion;
                Medicamento.CasaProductora = info.CasaProductora;
                Medicamento.Precio = info.Precio;
                Singleton.Instance.Compra.InsertarInicio(Medicamento);
                Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.Compra.ObtenerValor(0).ID - 1).Existencia--;
            }
            else if (Singleton.Instance.Pedido != null)
            {
                int i = 0;
                for(i = 0; i < Singleton.Instance.Compra.contador; i++)
                {
                    if (Singleton.Instance.Compra.ObtenerValor(i).Nombre == Singleton.Instance.Pedido.ObtenerValor(0).Nombre)
                    {
                        if (Singleton.Instance.Pedido.ObtenerValor(i).Existencia > 0)
                        {
                            Singleton.Instance.Compra.ObtenerValor(i).Existencia++;
                            Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.Compra.ObtenerValor(i).ID - 1).Existencia--;
                            if (Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.Compra.ObtenerValor(i).ID - 1).Existencia == 0)
                            {
                                Singleton.Instance.Actualizar();
                                ViewBag.Mensaje = "No quedan mas existencias de " + Singleton.Instance.Compra.ObtenerValor(i).Nombre;
                            }
                            return View("Pedido");
                        }
                    }
                }
                InfoFarmaco Medicamento = new InfoFarmaco();
                InfoFarmaco info = Singleton.Instance.Pedido.ObtenerValor(0);
                Medicamento.Existencia = 1;
                Medicamento.Nombre = info.Nombre;
                Medicamento.ID = info.ID;
                Medicamento.Descripcion = info.Descripcion;
                Medicamento.CasaProductora = info.CasaProductora;
                Medicamento.Precio = info.Precio;
                Singleton.Instance.Compra.InsertarFinal(Medicamento);
                Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.Compra.ObtenerValor(i).ID - 1).Existencia--;

            }
            return View("Pedido");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Metodo de busqueda
        public IActionResult Busqueda(string cadena)
        {

            if (cadena != null)
            {
                ListaDoble<InfoFarmaco> infoFarmacos = null;
                cadena = cadena.ToLower();
                int posicion = Singleton.Instance.Indice.Buscar(cadena);
                InfoFarmaco infoFarmaco = Singleton.Instance.ListaFarmacos.ObtenerValor(posicion - 1);
                infoFarmacos = new ListaDoble<InfoFarmaco>();
                infoFarmacos.InsertarInicio(infoFarmaco);
                Singleton.Instance.Pedido = infoFarmacos;
                return View("Prueba", Singleton.Instance.Pedido);
            }
            else
            {
                return View("Prueba");
            }

        }

        // Método para leer archivo CSV

        [HttpGet]
        public IActionResult Index(ListaDoble<InfoFarmaco> ListaFarmacos = null)
        {
            if(ListaFarmacos.inicio != null)
            {
                Singleton.Instance.ListaFarmacos = ListaFarmacos;
                Singleton.Instance.Actualizar(); // método que crea el árbol binario dentro del Arbol Indice
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
