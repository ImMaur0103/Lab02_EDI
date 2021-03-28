using System;
using System.Collections.Generic;
using System.Text;
using ListaDobleEnlace;
using ArbolAVL;

namespace Arbol
{
    public class Arbol<T>:Nodo<T>
    {
        ArbolAVL<T> ArbolAVL = new ArbolAVL<T>();

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
            Nodo<T> Raiz = actual;
            
            if(nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) < 0)
            {
                if (actual.izquierda == null)
                {
                    actual.izquierda = nuevo;
                }
                else
                {
                    actual.izquierda = InsertarNodo(actual.izquierda, nuevo);
                    if((ArbolAVL.CalcFe(actual.izquierda) - ArbolAVL.CalcFe(actual.derecha)) == 2){
                        if(nuevo.valor.Nombre.CompareTo(actual.izquierda.valor.Nombre) < 0)
                        {
                            Raiz = ArbolAVL.RotarIzquierda(actual);
                        }
                        else
                        {
                            Raiz = ArbolAVL.RDobleIzquierda(actual);
                        }
                    }
                }
            }
            else if (nuevo.valor.Nombre.CompareTo(actual.valor.Nombre) > 0)
            {
                if(actual.derecha == null)
                {
                    actual.derecha = nuevo;
                }
                else
                {
                    actual.derecha = InsertarNodo(actual.derecha, nuevo);
                    if((ArbolAVL.CalcFe(actual.derecha)-ArbolAVL.CalcFe(actual.izquierda)) == 2)
                    {
                        if (nuevo.valor.Nombre.CompareTo(actual.derecha.valor.Nombre) > 0)
                        {
                            Raiz = ArbolAVL.RotarDerecha(actual);
                        }
                        else
                        {
                            Raiz = ArbolAVL.RDobleDerecha(actual);
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            if ((actual.izquierda == null) && (actual.derecha != null))
            {
                actual.Fe = actual.derecha.Fe + 1;
            }
            else if ((actual.derecha == null) && (actual.izquierda != null))
            {
                actual.Fe = actual.izquierda.Fe + 1;
            }
            else
            {
                actual.Fe = Math.Max(ArbolAVL.CalcFe(actual.izquierda), ArbolAVL.CalcFe(actual.derecha)) + 1;
            }

            return Raiz;
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

        public Nodo<T> DeleteNodo(Nodo<T> actual, Nodo<T> Borrar)
        {
            if(Borrar.valor.Nombre.CompareTo(actual.valor.Nombre) < 0)
            {
                if(actual.izquierda.valor.Nombre == Borrar.valor.Nombre)
                {
                    if(actual.izquierda.izquierda == null && actual.izquierda.derecha == null)
                    {
                        actual.izquierda = null;
                    }
                    else if(actual.izquierda.izquierda != null && actual.izquierda.derecha != null)
                    {
                        Nodo<T> aux = actual.izquierda;
                        aux = Rearmar(aux, aux.derecha);
                        AjusteFeDerecha(ref aux);
                        aux = ArbolAVL.Balancear(aux);
                        actual.izquierda = aux;
                    }
                    else if(actual.izquierda.izquierda != null && actual.izquierda.derecha == null)
                    {
                        actual.izquierda = actual.izquierda.izquierda;
                    }
                    else if (actual.izquierda.izquierda == null && actual.izquierda.derecha != null)
                    {
                        actual.izquierda = actual.izquierda.derecha;
                    }
                    actual = ArbolAVL.Balancear(actual);
                }
                else
                {
                    actual.izquierda = DeleteNodo(actual.izquierda, Borrar);
                }
            }
            else if (Borrar.valor.Nombre.CompareTo(actual.valor.Nombre) > 0)
            {
                if (actual.derecha.valor.Nombre == Borrar.valor.Nombre)
                {
                    if (actual.derecha.izquierda == null && actual.derecha.derecha == null)
                    {
                        actual.derecha = null;
                    }
                    else if (actual.derecha.izquierda != null && actual.derecha.derecha != null)
                    {
                        Nodo<T> aux = actual.derecha;
                        aux = Rearmar(aux, aux.derecha);
                        AjusteFeDerecha(ref aux);
                        aux = ArbolAVL.Balancear(aux);
                        actual.derecha = aux;
                    }
                    else if (actual.derecha.izquierda != null && actual.derecha.derecha == null)
                    {
                        actual.izquierda = actual.izquierda.izquierda;
                    }
                    else if (actual.derecha.izquierda == null && actual.derecha.derecha != null)
                    {
                        actual.izquierda = actual.izquierda.derecha;
                    }
                    actual = ArbolAVL.Balancear(actual);
                }
                else
                {
                    actual.derecha = DeleteNodo(actual.derecha, Borrar);
                }
            }

            if ((actual.izquierda == null) && (actual.derecha != null))
            {
                actual.Fe = actual.derecha.Fe - 1;
            }
            else if ((actual.derecha == null) && (actual.izquierda != null))
            {
                actual.Fe = actual.izquierda.Fe - 1;
            }
            else
            {
                actual.Fe = Math.Max(ArbolAVL.CalcFe(actual.izquierda), ArbolAVL.CalcFe(actual.derecha)) + 1;
            }

            Nodo<T> Raiz = actual;
            return Raiz;
        }

        private Nodo<T> Rearmar(Nodo<T> raiz, Nodo<T> auxiliar)
        {
            Nodo<T> RaizAux = new Nodo<T>();

            if (auxiliar.izquierda != null)
            {
                RaizAux = Rearmar(auxiliar, auxiliar.izquierda);
                if (RaizAux.derecha != null)
                {
                    auxiliar.izquierda = RaizAux.derecha;
                    RaizAux.derecha = auxiliar;
                }
                else
                {
                    auxiliar.izquierda = null;
                    RaizAux.derecha = auxiliar;
                }
            }
            else
            {
                RaizAux = auxiliar;
            }
            RaizAux.izquierda = raiz.izquierda;
            return RaizAux;
        }

        private void AjusteFeIzquierda(ref Nodo<T> raiz)
        {
            if(raiz.izquierda == null && raiz.derecha == null)
            {
                raiz.Fe = 0;
                return;
            }
            else if(raiz.izquierda == null && raiz.derecha != null)
            {
                Nodo<T> aux = raiz.derecha;
                AjusteFeDerecha(ref aux);
                raiz.Fe = raiz.derecha.Fe + 1;
            }
            else if(raiz.izquierda != null && raiz.derecha == null)
            {
                Nodo<T> aux = raiz.izquierda;
                AjusteFeIzquierda(ref aux);
                raiz.Fe = raiz.izquierda.Fe + 1;
            }
            else if(raiz.izquierda != null && raiz.derecha != null)
            {
                Nodo<T> AuxI = raiz.izquierda;
                Nodo<T> AuxD = raiz.derecha;
                AjusteFeDerecha(ref AuxD);
                AjusteFeIzquierda(ref AuxI);
                raiz.Fe = Math.Max(raiz.izquierda.Fe, raiz.derecha.Fe) + 1;
            }
        }

        private void AjusteFeDerecha(ref Nodo<T> raiz)
        {
            if (raiz.izquierda == null && raiz.derecha == null)
            {
                raiz.Fe = 0;
                return;
            }
            else if (raiz.izquierda == null && raiz.derecha != null)
            {
                Nodo<T> aux = raiz.derecha;
                AjusteFeDerecha(ref aux);
                raiz.Fe = raiz.derecha.Fe + 1;
            }
            else if (raiz.izquierda != null && raiz.derecha == null)
            {
                Nodo<T> aux = raiz.izquierda;
                AjusteFeIzquierda(ref aux);
                raiz.Fe = raiz.izquierda.Fe + 1;
            }
            else if (raiz.izquierda != null && raiz.derecha != null)
            {
                Nodo<T> AuxI = raiz.izquierda;
                Nodo<T> AuxD = raiz.derecha;
                AjusteFeDerecha(ref AuxD);
                AjusteFeIzquierda(ref AuxI);
                raiz.Fe = Math.Max(raiz.izquierda.Fe, raiz.derecha.Fe) + 1;
            }
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
