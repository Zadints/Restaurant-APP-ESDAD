using App.BackEnd.Domain.Nodo;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services.ESDAD
{
    class Colas<T>
    {
        public NodoSimple<T> Frente { get; set; }
        public NodoSimple<T> Final { get; set; }

        public int Contador { get; set; }

        public Colas()
        {
            Frente = null;
            Final = null;
            Contador = 0;
        }

        public void Queue(T dato)
        {
            NodoSimple<T> nuevoNodo = new NodoSimple<T>(dato);
            if(Frente == null)
            {
                Frente = nuevoNodo;
                Final = nuevoNodo;
            }
            else
            {
                Final.Siguiente = nuevoNodo;
                Final = nuevoNodo;
            }

            Contador++;
        }

        public T Dequeue()
        {
            if (Frente == null)
            {
                return default(T);
            }

            T dato = Frente.Dato;
            Frente = Frente.Siguiente;
            Contador--;

            if(Frente == null)
            {
                Final = null;
            }

            return dato;
        }

        public T Peek()
        {
            if(Frente == null)
            {
                throw new InvalidOperationException("La cola está vacía.");
            }

            T dato = Frente.Dato;
            return dato;
        }
    }
}
