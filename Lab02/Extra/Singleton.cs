using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arbol;
using ListaDobleEnlace;
using Lab02.Models;

namespace Lab02.Extra
{
    public sealed class Singleton
    {
        private readonly static Singleton instance = new Singleton();
        //public ListaDoble<Jugador> listaDoble;
        public Arbol<Arbol.Farmaco> Indice;
        public ListaDoble<InfoFarmaco> ListaFarmacos; 

        private Singleton()
        {
            Indice = new Arbol<Arbol.Farmaco>();
            ListaFarmacos = new ListaDoble<InfoFarmaco>();
            //listaDoble = new ListaDoble<Jugador>();
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
