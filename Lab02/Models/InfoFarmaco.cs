using CsvHelper.Configuration.Attributes; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab02.Models
{
    public class InfoFarmaco
    {
        [Index (0)]
        public int ID { get; set; }
        [Index(1)]
        public string Nombre { get; set; }
        [Index(2)]
        public string Descripcion { get; set; }
        [Index(3)]
        public string CasaProductora { get; set; }
        [Index(4)]
        public double Precio { get; set; }
        [Index(5)]
        public int Existencia { get; set; }
    }
}
