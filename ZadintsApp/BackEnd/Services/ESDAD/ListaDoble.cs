using App.BackEnd.Domain.Nodo;

namespace App.BackEnd.Services.ESDAD
{
    public class ListaDoble<T>
    {
        public NodoDoble<T> Cabeza { get; set; }
        public NodoDoble<T> Cola { get; set; }

        public ListaDoble()
        {
            Cabeza = null;
            Cola = null;
        }

        public void InsertHead(T Content)
        {
            NodoDoble<T> newNodo = new NodoDoble<T>(Content);

            if (Cabeza == null)
            {
                Cabeza = Cola = newNodo;
                newNodo.Siguiente = Cabeza;
                newNodo.Anterior = Cola;  
                return;
            }


            newNodo.Siguiente = Cabeza;            
            newNodo.Anterior = Cola;
            Cabeza.Anterior = newNodo;
            Cola.Siguiente = newNodo;
            Cabeza = newNodo;
        }


        public void InsertLast(T Content)
        {
            NodoDoble<T> newNodo = new NodoDoble<T>(Content);
            if (Cabeza == null)
            {
                Cabeza = Cola = newNodo;
                newNodo.Siguiente = Cabeza;
                newNodo.Anterior = Cola;
                return;
            }
            newNodo.Siguiente = Cabeza;
            newNodo.Anterior = Cola;
            Cola.Siguiente = newNodo;
            Cabeza.Anterior = newNodo;
            Cola = newNodo;
        }

        


    }
}
