using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;


namespace App.BackEnd.Domain.Nodo
{
    public class NodoDoble<T>
    {
        public T Dato { get; set; }

        public NodoDoble<T>? Siguiente { get; set; }
        public NodoDoble<T>? Anterior { get; set; }
        public NodoDoble(T dato)
        {
            Dato = dato;
            Siguiente = null;
            Anterior = null;
        }
    }
}


