using System;
using System.Collections.Generic;
using System.Text;
using ListaDobleEnlace;

namespace Arbol
{
    public class Arbol<T>:Nodo<T>
    {
        public Nodo<T> raiz;

        //constructor 
        public Arbol()
        {
            raiz = null; 
        }

        ~Arbol() { }

        // Insertar nodos en el árbol 
        public void Insertar(Farmaco valor)
        {
            Nodo<T> NuevoNodo = new Nodo<T>();
            NuevoNodo.valor = valor;
            NuevoNodo.izquierda = null;
            NuevoNodo.derecha = null;

            if (raiz == null)
            {
                raiz = NuevoNodo;
            }
            else
            {
                raiz = InsertarNodo(raiz, NuevoNodo);
            }
        }

        public T Mayor<T>(T valor1, T valor2) where T : IComparable
        {
            if (valor1.CompareTo(valor2) > 0) return valor1;
            return valor2;
        }

        private Nodo<T> InsertarNodo(Nodo<T> actual, Nodo<T> nuevo)
        {
            if (nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) > 0)
            {
                if(actual.derecha == null)
                {
                    actual.derecha = nuevo;
                    return actual;
                }
                else
                {
                    actual.derecha = InsertarNodo(actual.derecha, nuevo);
                    return actual;
                }
            }
            else if(nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) < 0)
            {
                if (actual.izquierda == null)
                {
                    actual.izquierda = nuevo;
                    return actual;
                }
                else
                {
                    actual.izquierda = InsertarNodo(actual.izquierda, nuevo);
                    return actual;
                }
            }
            else
            {
                return null;
            }
        }

        public int Buscar(string nombre)
        {
            Nodo<T> recorrer = raiz;
            bool encontrar = false;
            while(recorrer != null || encontrar == false)
            {
                if(nombre == recorrer.valor.Nombre)
                {
                    encontrar = true; 
                }
                else
                {
                    if(nombre.CompareTo(recorrer.valor.Nombre) > 0)
                    {
                        recorrer = recorrer.derecha;
                    }
                    else
                    {
                        recorrer = recorrer.izquierda;
                    }
                }
            }
            return recorrer.valor.Numero_Linea;
        }

        public void Preorden(Nodo<Farmaco> raiz, ListaDoble<Farmaco> ListaInventario)
        {
            if (raiz!= null)
            {
                ListaInventario.InsertarInicio(raiz.valor);
                Preorden(raiz.izquierda, ListaInventario);
                Preorden(raiz.derecha, ListaInventario);
            }
        }
    }
}
