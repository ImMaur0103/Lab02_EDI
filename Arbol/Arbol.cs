using System;
using System.Collections.Generic;
using System.Text;
using ListaDobleEnlace;

namespace Arbol
{
    public class Arbol<T>:Nodo<T>
    {
        public Nodo<T> raiz;
        public int contador;

        //constructor 
        public Arbol()
        {
            raiz = null;
            contador = 0;
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
            contador++;
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
            while(recorrer != null && encontrar == false)
            {
                string valor = recorrer.valor.Nombre;
                valor = valor.ToLower();
                if(nombre == valor)
                {
                    encontrar = true; 
                }
                else
                {
                    if(nombre.CompareTo(recorrer.valor.Nombre) > 0)
                    {
                        recorrer = recorrer.derecha;
                        encontrar = false;
                    }
                    else
                    {
                        recorrer = recorrer.izquierda;
                        encontrar = false; 
                    }
                }
            }
            if(recorrer == null)
            {
                return 0;
            }
            return recorrer.valor.Numero_Linea;
        }

        public void Delete()
        {
            raiz = null;
            contador = 0;
        }

        //Verifica el estado del índice, por lo que guarda los valores dentro de una lista tipo FARMACO
        public void Preorden(Nodo<Farmaco> raiz, ref ListaDoble<Farmaco> ListaInventario)
        {
            if (raiz!= null)
            {
                ListaInventario.InsertarInicio(raiz.valor);
                Preorden(raiz.izquierda, ref ListaInventario);
                Preorden(raiz.derecha, ref ListaInventario);
            }
        }

        public void InOrden(Nodo<Farmaco> raiz,ref ListaDoble<Farmaco> ListaInventario)
        {
            if(raiz!= null)
            {
                InOrden(raiz.izquierda,ref ListaInventario);
                ListaInventario.InsertarInicio(raiz.valor);
                InOrden(raiz.derecha,ref ListaInventario);
            }
        }

        public void PostOrden(Nodo<Farmaco> raiz,ref ListaDoble<Farmaco> ListaInventario)
        {
            if(raiz != null)
            {
                PostOrden(raiz.izquierda, ref ListaInventario);
                PostOrden(raiz.derecha, ref ListaInventario);
                ListaInventario.InsertarInicio(raiz.valor);
            }
        }
    }
}
