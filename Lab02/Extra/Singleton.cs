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
        public Arbol<Farmaco> Indice;
        public ListaDoble<Farmaco> InventarioTipoArbolp;
        public ListaDoble<InfoFarmaco> ListaFarmacos;
        public ListaDoble<Farmaco> Inventario;

        private Singleton()
        {
            Indice = new Arbol<Arbol.Farmaco>();
            ListaFarmacos = new ListaDoble<InfoFarmaco>();
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
