using System;
using System.Collections.Generic;
using System.Text;

namespace ListaDobleEnlace
{
    public class ListaDoble<T>:Nodo<T>
    {
        private Nodo<T> inicio = new Nodo<T>();
        private Nodo<T> fin = new Nodo<T>();
        int contador;

        public ListaDoble()
        {
            inicio = null;
            fin = null;
            contador = 0;
        }

        ~ListaDoble() { }

        bool ListaVacia()
        {
            return contador == 0;
        }

        public void InsertarInicio(T NuevoValor)
        {
            Nodo<T> nuevoNodo = new Nodo<T>();
            nuevoNodo.Valor = NuevoValor;

            if (ListaVacia())
            {
                inicio = nuevoNodo;
                fin = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = inicio;
                inicio.Anterior = nuevoNodo;
                inicio = nuevoNodo;
            }
            contador++;
            return;
        }

        Nodo<T> ExtraerInicio()
        {
            Nodo<T> temporal = inicio;

            if (!ListaVacia())
            {
                inicio = inicio.Siguiente;
                inicio.Anterior = null;
                if (contador == 1)
                {
                    fin = inicio;
                }
                contador--;
            }
            return temporal;
        }

        Nodo<T> ExtraerFinal()
        {
            Nodo<T> temporal = fin;

            if (!ListaVacia())
            {
                if (contador == 1)
                {
                    fin = fin.Siguiente;
                    inicio = fin;
                }
                else
                {
                    fin = fin.Anterior;
                    fin.Siguiente = null;
                }
                contador--;
            }
            return temporal;
        }

        Nodo<T> ExtraerEnPosicion(int posicion)
        {
            Nodo<T> temporal = inicio;

            if (!ListaVacia())
            {
                if ((contador == 1) && (posicion == 0))
                {
                    return ExtraerInicio();
                }
                else
                {
                    if (posicion >= contador)
                    {
                        return ExtraerFinal();
                    }
                    else
                    {
                        Nodo<T> auxiliar = inicio;
                        int pos = 1;

                        while ((pos < posicion))
                        {
                            auxiliar = auxiliar.Siguiente;
                        }
                        auxiliar.Anterior.Siguiente = auxiliar.Siguiente;
                        auxiliar.Siguiente.Anterior = auxiliar.Anterior;
                        contador--;
                    }
                }
            }
            return temporal;
        }

        T ObtenerValor(int posicion)
        {
            if (posicion >= 0 && posicion < contador)
            {

                Nodo<T> temporal = inicio;
                int ubicacion = 0;

                while (ubicacion < posicion)
                {
                    temporal = temporal.Siguiente;
                }
                return temporal.Valor;
            }
            return default(T);
        }
    }
}
