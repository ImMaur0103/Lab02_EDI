using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol
{
    class Arbol<T>:Nodo<T>
    {
        private Nodo<T> arbol = new Nodo<T>();
        int contador;

        public Arbol()
        {
            arbol = null;
            contador = 0;
        }

        ~Arbol() { }

        public void Insertar(Nodo<T> nodo, T valor)
        {
            if (arbol == null)
            {
                Nodo<T> nuevoNodo = CrearNodo(valor);
                arbol = nuevoNodo;
            }
            else
            {
                T valorRaiz = arbol.valor;
                if (nodo.compare)
                {

                }
            }
        }

    }
}
