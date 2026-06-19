using App.BackEnd.Domain.Nodo;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services.ESDAD
{
    class ArbolesBinarios<T>
    {
        public NodoArbol<T> raiz { get; set; }

        public bool EstaVacio()
        {
            return raiz == null;
        }

        public void CrearRaiz(T dato)
        {
            if (!EstaVacio())
                throw new InvalidOperationException("El arbol ya tiene una raiz");
            raiz = new NodoArbol<T>(dato);
        }

        public void AgregarHijoIzquierdo(NodoArbol<T> Padre, T dato)
        {
            if (Padre == null)
                throw new InvalidOperationException("El padre no existe");
            if (Padre.izquierda != null)
                throw new InvalidOperationException("El padre tiene un hijo izquierdo");
            Padre.izquierda = new NodoArbol<T>(dato);
        }

        public void AgregarHijoDerecho(NodoArbol<T> Padre, T dato)
        {
            if (Padre == null)
                throw new InvalidOperationException("El padre no existe");
            if (Padre.derecha != null)
                throw new InvalidOperationException("El padre tiene un hijo derecha");
            Padre.derecha = new NodoArbol<T>(dato);
        }

        public int ContarNodos()
        {
            return ContarNodos(raiz);
        }

        private int ContarNodos(NodoArbol<T> nodo)
        {
            if (nodo == null)
                return 0;
            return 1 + ContarNodos(nodo.izquierda) + ContarNodos(nodo.derecha);
        }

        public int ContarHojas()
        {
            return ContarHojas(raiz);
        }

        public int ContarHojas(NodoArbol<T> nodo)
        {
            if (nodo == null)
                return 0;
            if (nodo.EsHoja())
                return 1;
            return ContarHojas(nodo.izquierda) + ContarHojas(nodo.derecha);
        }

        public int Altura()
        {
            return Altura(raiz);
        }

        private int Altura(NodoArbol<T> nodo)
        {
            if (nodo == null)
                return 0;
            int AlturaIzquierda = Altura(nodo.izquierda);
            int AlturaDerecha = Altura(nodo.derecha);

            return 1 + Math.Max(AlturaIzquierda, AlturaDerecha);
        }



        public ListaSimple<T> InOrden()
        {
            ListaSimple<T> lista = new();
            InOrden(raiz, lista);
            return lista;
        }

        private void InOrden(NodoArbol<T> nodo, ListaSimple<T> lista)
        {
            if (nodo == null)
                return;

            InOrden(nodo.izquierda, lista);
            lista.InsertarCabeza(nodo.dato);
            InOrden(nodo.derecha, lista);
        }



        public void Insertar(T dato, Comparison<T> comparador, Action<T, T> actualizar)
        {
            raiz = Insertar(raiz, dato, comparador, actualizar);
            
        }
        private NodoArbol<T> Insertar(
            NodoArbol<T> nodo,
            T dato,
            Comparison<T> comparador,
            Action<T, T> actualizar)
        {
            if (nodo == null)
                return new NodoArbol<T>(dato);

            int cmp = comparador(dato, nodo.dato);

            if (cmp < 0)
            {
                nodo.izquierda = Insertar(nodo.izquierda, dato, comparador, actualizar);
            }
            else if (cmp > 0)
            {
                nodo.derecha = Insertar(nodo.derecha, dato, comparador, actualizar);
            }
            else
            {
                actualizar?.Invoke(nodo.dato, dato);
            }

            return nodo;
        }

        public void Reordenar(Comparison<T> comparador)
        {
            ListaSimple<T> elementos = InOrden();
            NodoSimple<T> actual = elementos.Cabeza;

            raiz = null;

            while (actual != null)
            {
                Insertar(actual.Dato, comparador, null);
                actual = actual.Siguiente;
            }
        }

        public T Buscar(T dato, Comparison<T> comparador, ref int comparaciones, ref bool encontrado)
        {
            comparaciones = 0;
            encontrado = false;
            return Buscar(raiz, dato, comparador, ref comparaciones, ref encontrado);
        }
        private T Buscar(NodoArbol<T> nodo, T dato, Comparison<T> comparador, ref int comparaciones, ref bool encontrado)
        {
            if (nodo == null)
            {
                encontrado = false;
                return default(T);
            }
            comparaciones++;

            int resultado = comparador(dato, nodo.dato);
            if (resultado == 0)
            {
                encontrado = true;
                return nodo.dato;
            }
            if (resultado < 0)
                return Buscar(nodo.izquierda, dato, comparador, ref comparaciones, ref encontrado);
            return Buscar(nodo.izquierda, dato, comparador, ref comparaciones, ref encontrado);
        }

        public void Eliminar(T dato, Comparison<T> comparador)
        {
            raiz = Eliminar(raiz, dato, comparador);
        }

        private NodoArbol<T> Eliminar(NodoArbol<T> nodo, T dato, Comparison<T> comparador)
        {
            if (nodo == null)
                return null;

            int resultado = comparador(dato, nodo.dato);

            if (resultado < 0)
                nodo.izquierda = Eliminar(nodo.izquierda, dato, comparador);
            else if (resultado > 0)
                nodo.derecha = Eliminar(nodo.derecha, dato, comparador);
            else
            {
                //Caso 1 
                if (nodo.EsHoja())
                    return null;
                //Caso 2
                if (nodo.izquierda == null)
                    return nodo.derecha;

                if (nodo.derecha == null)
                    return nodo.izquierda;

                //caso 3
                NodoArbol<T> sucesor = ObtenerMinimo(nodo.derecha);

                nodo.dato = sucesor.dato;

                nodo.derecha = Eliminar(nodo.derecha, sucesor.dato, comparador);
            }
            return nodo;
        }

        private NodoArbol<T> ObtenerMinimo(NodoArbol<T> nodo)
        {
            while (nodo.izquierda != null)
                nodo = nodo.izquierda;
            return nodo;
        }


    }
}
