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
        public Arbol<Farmaco> SinExistencias;



        public ListaDoble<InfoFarmaco> ListaFarmacosOrdenados;
        public ListaDoble<InfoFarmaco> ListaFarmacos;
        public ListaDoble<Farmaco> Ordenamiento; 
        public ListaDoble<InfoFarmaco> Pedido;
        public ListaDoble<InfoFarmaco> Compra;
        public ListaDoble<Farmaco> InventarioTipoArbol;
        public ListaDoble<Farmaco> Inventario; // Lista no se usa

        private Singleton()
        {
            //Arboles
            Indice = new Arbol<Farmaco>();
            SinExistencias = new Arbol<Farmaco>();

            //Listas
            Ordenamiento = new ListaDoble<Farmaco>();
            ListaFarmacosOrdenados = new ListaDoble<InfoFarmaco>();
            ListaFarmacos = new ListaDoble<InfoFarmaco>();
            Pedido = new ListaDoble<InfoFarmaco>();
            Compra = new ListaDoble<InfoFarmaco>();
            InventarioTipoArbol = new ListaDoble<Farmaco>();
        }

        public void Actualizar()
        {
            if(Indice.raiz != null)
            {
                Indice.Delete();
                SinExistencias.Delete();
            }
            for (int i = 0; i < ListaFarmacos.contador; i++)
            {

                Farmaco NuevoFarmaco = new Farmaco();
                InfoFarmaco FarmacoNuevo = new InfoFarmaco();
                FarmacoNuevo = ListaFarmacos.ObtenerValor(i);

                NuevoFarmaco.Nombre = FarmacoNuevo.Nombre;
                NuevoFarmaco.Numero_Linea = FarmacoNuevo.ID;

                if (FarmacoNuevo.Existencia != 0)
                {
                    Indice.Insertar(NuevoFarmaco);
                }
                else
                {
                    SinExistencias.Insertar(NuevoFarmaco);
                }

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
