using System;

namespace Arbol
{
    public class Nodo<T>
    {
        // Valor del nodo, el cual es el NoLinea y Nombre del fármaco
        public Farmaco valor { get; set; }

        //Posiciones del árbol binario
        public Nodo<T> derecha { get; set; }
        public Nodo<T> izquierda { get; set; }

        //Factor de equilibrio, propio del árbol AVL
        public int Fe;

        // constructor de la clase Nodo
        public Nodo()
        {
            Fe = 0; 
            derecha = null;
            izquierda = null;
        }
        
       // public Nodo<T> raiz; 

        ~Nodo() { }
    }
}
