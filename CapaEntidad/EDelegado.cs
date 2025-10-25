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
        public int IdPersona { get; set; }
        public int IdEleccion { get; set; }
        public int IdMesa { get; set; }
        public string FechaRegistro { get; set; }
        public EPersona RefPersona { get; set; }
    }
}
