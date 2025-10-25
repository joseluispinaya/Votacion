using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EResultado
    {
        public int IdResultado { get; set; }
        public int IdEleccion { get; set; }
        public int IdMesa { get; set; }
        public int IdPartido { get; set; }
        public int Votos { get; set; }
        public int IdDelegado { get; set; }
        public string FechaRegistro { get; set; }
    }
}
