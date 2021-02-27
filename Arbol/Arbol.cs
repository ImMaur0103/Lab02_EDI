using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol
{
    public class Arbol<T>:Nodo<T>
    {
       // Farmaco farmaco = new Farmaco();

        private Nodo<T> arbol = new Nodo<T>();
        int contador;

        public Arbol()
        {
            arbol = null;
            contador = 0;
        }

        ~Arbol() { }

        public T Mayor<T>(T valor1, T valor2) where T : IComparable
        {
            if (valor1.CompareTo(valor2) > 0) return valor1;
            return valor2;
        }

        // Insertar nodos en el árbol 
        public void Insertar(Nodo<T> arbol, T valor)
        {
            if (arbol == null)
            {
                Nodo<T> nuevoNodo = CrearNodo(valor);
                arbol = nuevoNodo;
            }
            else
            {
                T valorRaiz = arbol.valor;

                // se asigna como mayor al valorRaiz
                string mayor = valorRaiz.ToString();
                string menor = valor.ToString();

                string comparacion = Mayor<string>(mayor, menor);

                // solo se compara mayor y menor porque no hay valores repetidos dentro del árbol binario
                if (comparacion == mayor)
                {
                    Insertar(arbol.derecha, valor);
                }
                else
                {
                    Insertar(arbol.izquierda, valor);
                }
            }
        }

    }
}
