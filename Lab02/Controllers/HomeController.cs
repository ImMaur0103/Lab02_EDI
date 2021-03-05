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
using System.Text;

namespace Lab02.Controllers
{
    public class HomeController : Controller
    {
        public double total = 0;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string option, string ordenamiento)
        {
            switch (ordenamiento)
            {
                case "PreOrden":
                    Singleton.Instance.Ordenamiento = ListaToArbol(Singleton.Instance.Indice, 1);
                    break;
                case "InOrden":
                    Singleton.Instance.Ordenamiento = ListaToArbol(Singleton.Instance.Indice, 2);
                    break;
                case "PostOrden":
                    Singleton.Instance.Ordenamiento = ListaToArbol(Singleton.Instance.Indice, 3);
                    break;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            ListaToArbol(Singleton.Instance.Indice, 1);
            return ExportarCSV(Singleton.Instance.Ordenamiento);
        }

        public IActionResult Buscar()
        {
            return View("Pedido");
        }
        
        public IActionResult Inventario(bool Rellenar)
        {
            if (true){

            }
            return View("Prueba", Singleton.Instance.ListaFarmacos);
        }

        //View con codigo de funcionamiento
        public IActionResult Caja()
        {
            total = Math.Round(Total(Singleton.Instance.Compra), 2);
            ViewData["Total"] = "TOTAL: $ " + total.ToString(); 
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
                        if (Singleton.Instance.Pedido.ObtenerValor(0).Existencia > 0)
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

        double Total(ListaDoble<InfoFarmaco> ListaCompra)
        {
            InfoFarmaco info = new InfoFarmaco();
            
            for (int i = 0; i < ListaCompra.contador; i++)
            {
                info = ListaCompra.ObtenerValor(i);
                string precioString = info.Precio;
                precioString = precioString.Trim('$');
                double precio = Convert.ToDouble(precioString);
                total += precio;
            }

            return total; 
        }

        void RellenarInventario()
        {

        }

        public IActionResult OrdenAlfabetico()
        {
            if(Singleton.Instance.ListaFarmacos.inicio != null)
            {
                Singleton.Instance.ListaFarmacosOrdenados.Vaciar();
                Singleton.Instance.InventarioTipoArbol.Vaciar();
                Singleton.Instance.Indice.InOrden(Singleton.Instance.Indice.raiz,ref Singleton.Instance.InventarioTipoArbol);
                if(Singleton.Instance.SinExistencias.raiz != null)
                {
                    Singleton.Instance.SinExistencias.InOrden(Singleton.Instance.SinExistencias.raiz,ref Singleton.Instance.InventarioTipoArbol);
                }
                for(int i = 0; i < Singleton.Instance.InventarioTipoArbol.contador; i++)
                {
                    Singleton.Instance.ListaFarmacosOrdenados.InsertarInicio(Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.InventarioTipoArbol.ObtenerValor(i).Numero_Linea - 1));
                }
            }
            else
            {
                return View("Prueba", Singleton.Instance.ListaFarmacos);
            }
            return View("Prueba", Singleton.Instance.ListaFarmacosOrdenados);
        }

        public IActionResult PreOrder()
        {
            if (Singleton.Instance.ListaFarmacos.inicio != null)
            {
                Singleton.Instance.ListaFarmacosOrdenados.Vaciar();
                Singleton.Instance.InventarioTipoArbol.Vaciar();
                Singleton.Instance.Indice.Preorden(Singleton.Instance.Indice.raiz, ref Singleton.Instance.InventarioTipoArbol);
                if (Singleton.Instance.SinExistencias.raiz != null)
                {
                    Singleton.Instance.SinExistencias.Preorden(Singleton.Instance.SinExistencias.raiz, ref Singleton.Instance.InventarioTipoArbol);
                }
                for (int i = 0; i < Singleton.Instance.InventarioTipoArbol.contador; i++)
                {
                    Singleton.Instance.ListaFarmacosOrdenados.InsertarInicio(Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.InventarioTipoArbol.ObtenerValor(i).Numero_Linea - 1));
                }
            }
            else
            {
                return View("Prueba", Singleton.Instance.ListaFarmacos);
            }
            return View("Prueba", Singleton.Instance.ListaFarmacosOrdenados);
        }

        public IActionResult PostOrder()
        {
            if (Singleton.Instance.ListaFarmacos.inicio != null)
            {
                Singleton.Instance.ListaFarmacosOrdenados.Vaciar();
                Singleton.Instance.InventarioTipoArbol.Vaciar();
                Singleton.Instance.Indice.PostOrden(Singleton.Instance.Indice.raiz, ref Singleton.Instance.InventarioTipoArbol);
                if (Singleton.Instance.SinExistencias.raiz != null)
                {
                    Singleton.Instance.SinExistencias.PostOrden(Singleton.Instance.SinExistencias.raiz, ref Singleton.Instance.InventarioTipoArbol);
                }
                for (int i = 0; i < Singleton.Instance.InventarioTipoArbol.contador; i++)
                {
                    Singleton.Instance.ListaFarmacosOrdenados.InsertarInicio(Singleton.Instance.ListaFarmacos.ObtenerValor(Singleton.Instance.InventarioTipoArbol.ObtenerValor(i).Numero_Linea - 1));
                }
            }
            else
            {
                return View("Prueba", Singleton.Instance.ListaFarmacos);
            }
            return View("Prueba", Singleton.Instance.ListaFarmacosOrdenados);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Metodo de busqueda 
        public IActionResult Busqueda(string cadena)
        {
            /* Funcion usada en el view para obterner un buscar mas estetico
            <form>
                <button name="BotonBuscar" asp-action="Busqueda"><i class="fas fa-searchfas fa-search"></i></button>
                @Html.TextBox("cadena", "", new { @class = "show", @placeholder = "Buscar por nombre", @size = "100"})
            </form>
            */
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

            //Lista que se utilizará para realizar cada tipo de ordenamiento.
            //Singleton.Instance.Ordenamiento = ListaToArbol(Singleton.Instance.Indice, 1);
        }

        ListaDoble<Farmaco> ListaToArbol(Arbol<Farmaco> ArbolFarmacos, int caso)
        {
            ListaDoble<Farmaco> ListaFarmacos = new ListaDoble<Farmaco>();

            switch (caso)
            {
                case 1:
                    ArbolFarmacos.Preorden(ArbolFarmacos, ListaFarmacos);
                    break;
                case 2:
                    ArbolFarmacos.InOrden(ArbolFarmacos, ListaFarmacos);
                    break;
                case 3:
                    ArbolFarmacos.PostOrden(ArbolFarmacos, ListaFarmacos);
                    break;
            }
            return ListaFarmacos;
        }


        // Método para exportar archivo CSV.
        public IActionResult ExportarCSV(ListaDoble<Farmaco> Lista)
        {
            Singleton.Instance.Indice.Preorden(Singleton.Instance.Indice, Singleton.Instance.Ordenamiento);
            var builder = new StringBuilder();
            builder.AppendLine("No_Linea,Nombre");
            foreach (var Farmaco in Lista)
            {
                builder.AppendLine($"{Farmaco.Numero_Linea},{Farmaco.Nombre}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "EstadoIndice.csv");
        }
    }
}
