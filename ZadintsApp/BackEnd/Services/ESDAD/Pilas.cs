using App.BackEnd.Domain.Nodo;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services.ESDAD
{
    class Pilas<T>
    {
        public int Contador { get; set; }
        public NodoSimple<T> Cima { get; set; }

        public void Push(T dato)
        {
            NodoSimple<T> nuevoNodo = new NodoSimple<T>(dato);
            nuevoNodo.Siguiente = Cima;
            Cima = nuevoNodo;
            Contador++;
        }

        public T Pop()
        {
            if (Cima == null)
            {
                throw new InvalidOperationException("No existe el nodo");
            }

            T dato = Cima.Dato;
            Cima = Cima.Siguiente;
            Contador--;
            return dato;
        }

        public T Peek()
        {
            if (Cima == null)
            {
                throw new InvalidOperationException("No existe el nodo");
            }
            return Cima.Dato;

        }

    }
}
