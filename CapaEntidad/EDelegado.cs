using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EDelegado
    {
        public int IdDelegado { get; set; }
        public string NombreCompleto { get; set; }
        public string CI { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Usuario { get; set; }
        public string ClaveHash { get; set; }
        public int IdMesa { get; set; }
        public string Rol { get; set; }
    }
}
