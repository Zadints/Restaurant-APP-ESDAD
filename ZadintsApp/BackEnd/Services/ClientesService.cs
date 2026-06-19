using App.BackEnd.Domain.Entities;
using App.BackEnd.Services.ESDAD;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services
{
    public class ClientesService
    {
        public static ListaDoble<ClienteEntity> _cliente = new ListaDoble<ClienteEntity>();

        public static string? AgregarCliente()
        {
            //agregar cleinte dependiendo su  Egreso actual y registrar en db
            return "";
        }

        public static string? EliminarCliente()
        {
            //eliminar dependiendo su nombre
            return "";
        }

        public static void CargarClientes(){
            //obtiene todos los clietnes de la db y los asigna a la lista doble _cliente
        }

        public static string? ActualizarCliente()
        {
            //Mover clientes dependiendo si su Egreso es mayor o menor al anterior o posterior en la lista doble _cliente

            return "";
        }


        public static ClienteEntity BuscarCliente(string clienteABuscar)
        {
        
            //Mover clientes dependiendo si su Egreso es mayor o menor al anterior o posterior en la lista doble _cliente
            ClienteEntity cliente = _cliente.Cabeza.Dato;
            return cliente;
        }
    }

}
