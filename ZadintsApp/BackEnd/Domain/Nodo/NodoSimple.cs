using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace App.BackEnd.Domain.Nodo
{
    public class NodoSimple<T>
    {
        public T Dato { get; set; }

        public NodoSimple<T> Siguiente { get; set; }

        public NodoSimple(T dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}


