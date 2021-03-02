using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab02.Models
{
    public class InfoFarmaco
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string CasaProductora { get; set; }
        public double Precio { get; set; }
        public int Existencia { get; set; }
    }
}
