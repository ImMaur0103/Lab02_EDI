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
            if (nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) > 0)//al ser mayor  se tiene que mover el arbol
            {
                actual = RestructurarArbol(actual, nuevo);
                actual = ArregloIndices(actual);
                return actual;
            }
            else if(nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) < 0)//si es menor se tiene que verificar si se va a la derecha o a la izquierda
            {
                if (actual.izquierda == null)
                {
                    actual.izquierda = nuevo;
                    return actual;
                }
                else if(actual.izquierda != null && actual.derecha == null)
                {
                    actual.derecha = nuevo;
                    actual.altura++;
                    return actual;
                }
                else if(actual.izquierda.altura > actual.derecha.altura)
                {
                    actual.derecha = InsertarNodo(actual.derecha, nuevo);
                    if (actual.altura < CalAlturas(actual.derecha) + 1)
                        actual.altura++;
                    return actual;
                }
                else
                {
                    actual.izquierda = InsertarNodo(actual.izquierda, nuevo);
                    if(actual.altura < CalAlturas(actual.derecha) + 1)
                        actual.altura++;
                    return actual;
                }
            }
            else
            {
                return null;
            }
        }

        private Nodo<T> RestructurarArbol(Nodo<T> actual, Nodo<T> nuevo)
        {
            if (actual.izquierda == null)
            {
                nuevo.izquierda = actual;
                return nuevo;
            }
            else if (actual.derecha == null)
            {
                nuevo.izquierda = actual.izquierda;
                actual.izquierda = null;
                nuevo.derecha = actual;
                return nuevo;
            }
            else if (CalAlturas(actual.izquierda) > CalAlturas(actual.derecha))
            {
                nuevo.izquierda = actual.izquierda;
                actual.izquierda = actual.derecha;
                actual.derecha = null;
                nuevo.derecha = actual;
                return nuevo;
            }
            else
            {
                if (CalAlturas(actual.izquierda.izquierda) + 1 > CalAlturas(actual.derecha))
                {
                    nuevo.derecha = actual.derecha;
                    actual.derecha = actual.izquierda.izquierda;
                    actual.izquierda.izquierda = null;
                    nuevo.izquierda = actual;
                    return nuevo;
                }
                else if (actual.izquierda.derecha != null)
                {
                    nuevo.derecha = actual.derecha;
                    actual.derecha = null;
                    nuevo.izquierda = actual;
                    return nuevo;
                }
                else
                {
                    nuevo.derecha = actual.derecha;
                    actual.derecha = null;
                    nuevo.izquierda = actual;
                    return nuevo;
                }
            }
        }

        private Nodo<T> ArregloIndices(Nodo<T> subArbol)
        {
            Nodo<T> Auxiliar = subArbol;

            if(Auxiliar.izquierda != null)
            {
                Auxiliar.altura = 0;
                Auxiliar.izquierda = ArregloIndices(Auxiliar.izquierda);
            }
            
            if (Auxiliar.derecha != null)
            {
                Auxiliar.derecha = ArregloIndices(Auxiliar.derecha);
                if(Auxiliar.altura < Auxiliar.derecha.altura + 1)
                    Auxiliar.altura = Auxiliar.derecha.altura + 1;
            }
            
            if(Auxiliar.derecha == null && Auxiliar.izquierda == null)
            {
                Auxiliar.altura = 0;
                return Auxiliar;
            }
            return Auxiliar;
        }

        private int CalAlturas(Nodo<T> obtener)
        {
            if(obtener == null)
            {
                return -1;
            }
            else
            {
                return obtener.altura;
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
            //ListaInventario = new ListaDoble<Farmaco>();
            if (raiz!= null)
            {
                ListaInventario.InsertarFinal(raiz.valor);
                Preorden(raiz.izquierda, ref ListaInventario);
                Preorden(raiz.derecha, ref ListaInventario);
            }
        }

        public void InOrden(Nodo<Farmaco> raiz,ref ListaDoble<Farmaco> ListaInventario)
        {
            if(raiz!= null)
            {
                InOrden(raiz.izquierda,ref ListaInventario);
                ListaInventario.InsertarFinal(raiz.valor);
                InOrden(raiz.derecha,ref ListaInventario);
            }
        }

        public void PostOrden(Nodo<Farmaco> raiz,ref ListaDoble<Farmaco> ListaInventario)
        {
            if(raiz != null)
            {
                PostOrden(raiz.izquierda, ref ListaInventario);
                PostOrden(raiz.derecha, ref ListaInventario);
                ListaInventario.InsertarFinal(raiz.valor);
            }
        }
    }
}
