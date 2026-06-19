using App.BackEnd.Domain.Entities;
using App.BackEnd.Services.ESDAD;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Services
{
    internal class MaquinasServices
    {
        private static readonly string _nombrePc = Environment.MachineName;
        public static ListaSimple<MaquinaConectadaEntity> _maquinasConectadas = new ListaSimple<MaquinaConectadaEntity>();
        private MaquinasServices() { }

        internal static async Task EstablecerConectado()
        {
             await DatabaseService.RegistrarEquipo(UsuarioService._usuarioActual.Correo, _nombrePc);
        }
        internal static void EstablecerDesconectado()
        {
            DatabaseService.EliminarEquipo(_nombrePc);
        }

        internal static async Task CargarMaquinasConectadas()
        {
            _maquinasConectadas = await DatabaseService.ObtenerEquiposConectados();
        }

    }
}
