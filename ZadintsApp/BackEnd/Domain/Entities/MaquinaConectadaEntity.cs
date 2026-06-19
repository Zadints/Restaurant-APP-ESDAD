using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.BackEnd.Domain.Entities
{
    internal class MaquinaConectadaEntity
    {
        public string CorreoUsuario { get; set; } = string.Empty;
        public string NombrePc { get; set; } = string.Empty;

        public MaquinaConectadaEntity() { }
        public override string ToString()
        {

            return $"💻 {NombrePc} |  👤{CorreoUsuario}";
        }
    }
    
    
}
