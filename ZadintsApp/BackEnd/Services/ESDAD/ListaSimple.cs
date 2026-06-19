using App.BackEnd.Domain.Nodo;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services.ESDAD
{
    public class ListaSimple<T>
    {
        public NodoSimple<T> Cabeza { get; set; }
        public NodoSimple<T> Cola { get; set; }

        public void InsertarCabeza(T content)
        {
            NodoSimple<T> newNodo = new NodoSimple<T>(content);
            newNodo.Siguiente = Cabeza;
            Cabeza = newNodo;
            if (Cola is null)
            {
                Cola = Cabeza;
            }
        }

        public void InsertarCola(T content)
        {
            NodoSimple<T> newNodo = new NodoSimple<T>(content);
            if (Cabeza is null)
            {
                Cabeza = newNodo;
                Cola = newNodo;
                return;
            }

            Cola.Siguiente = newNodo;
            Cola = newNodo;
        }

        public int Contar()
        {
            int count = 0;
            NodoSimple<T> actual = Cabeza;
            while (actual != null)
            {
                count++;
                actual = actual.Siguiente;
            }
            return count;
        }

        public T Buscar(Predicate<T> criterio)
        {           
            int count = 0;
            NodoSimple<T> current = Cabeza;
            while (current != null)
            {
                if (criterio(current.Dato))
                {
                    return current.Dato;
                }
                current = current.Siguiente;
                count++;
            }
            return default(T);
        }

        public void EliminarTodo()
        {
            Cabeza = null;
            Cola = null;
        }

        public bool Eliminar(Predicate<T> criterio)
        {
            if(Cabeza is null) return false;

            if (criterio(Cabeza.Dato))
            {
                if(Cabeza.Siguiente == null)
                {
                    Cabeza = null;
                    return true;
                }

                Cabeza = Cabeza.Siguiente;
                return true;
            }

            NodoSimple<T> current = Cabeza;

            while (current.Siguiente != null)
            {
                if(criterio(current.Siguiente.Dato))
                {
                    current.Siguiente = current .Siguiente.Siguiente;
                    return true;
                }
                current = current.Siguiente;

            }

            return false;
        }


    }
}
