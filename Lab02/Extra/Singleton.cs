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
        public ListaDoble<Arbol.Farmaco> InventarioTipoArbolp;
        public ListaDoble<InfoFarmaco> ListaFarmacos; 

        private Singleton()
        {
            Indice = new Arbol<Arbol.Farmaco>();
            ListaFarmacos = new ListaDoble<InfoFarmaco>();
            //listaDoble = new ListaDoble<Jugador>();
        }

        public void Actualizar()
        {
            for (int i = 0; i < ListaFarmacos.contador; i++)
            {

                Farmaco NuevoFarmaco = new Farmaco();
                InfoFarmaco FarmacoNuevo = new InfoFarmaco();
                FarmacoNuevo = ListaFarmacos.ObtenerValor(i);
                FarmacoNuevo = ListaFarmacos.ObtenerValor(i);

                NuevoFarmaco.Nombre = FarmacoNuevo.Nombre;
                NuevoFarmaco.Numero_Linea = FarmacoNuevo.ID;

                Indice.Insertar(NuevoFarmaco);
            }
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
