using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol
{
    public class Farmaco : IComparable<Farmaco>
    {
        public int Numero_Linea { get; set; }
        public string Nombre { get; set; }

        public int CompareTo(Farmaco farmaco)
        {
            return this.Nombre.CompareTo(farmaco.Nombre);
        }



    }
}
