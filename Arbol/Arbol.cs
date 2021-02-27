using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol
{
    public class Arbol<T>:Nodo<T>
    {
        Nodo<T> raiz;

        //constructor 
        public Arbol()
        {
            raiz = null; 
        }

        ~Arbol() { }

        // Insertar nodos en el árbol 
        public void Insertar(T valor)
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
            string valor1 = nuevo.valor.ToString();
            string valor2 = actual.valor.ToString();

            string mayor = Mayor<string>(valor1, valor2); // nuevo.valor compareTo(actual) > 0

            // solo se compara mayor y menor porque no hay valores repetidos dentro del árbol binario
            if (mayor == valor1)
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
            else if (mayor != valor1)
            {
                if(actual.izquierda == null)
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
    }
}
