using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Domain.Nodo
{
    class NodoArbol<T>
    {
        public T dato { get; set; }
        public NodoArbol<T> izquierda { get; set; }
        public NodoArbol<T> derecha { get; set; }

        public NodoArbol(T dato)
        {
            this.dato = dato;
            this.izquierda = null;
            this.derecha = null;
        }

        public bool EsHoja()
        {
            return izquierda == null && derecha == null;
        }
    }
}
