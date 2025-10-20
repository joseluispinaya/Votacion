using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EVotoEspecial
    {
        public int IdVotoEsp { get; set; }
        public int IdMesa { get; set; }
        public int Nulos { get; set; }
        public int Blancos { get; set; }
        public int IdDelegado { get; set; }
        public string FechaRegistro { get; set; }
    }
}
