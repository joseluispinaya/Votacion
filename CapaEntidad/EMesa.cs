using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EMesa
    {
        public int IdMesa { get; set; }
        public int IdRecinto { get; set; }
        public int IdEleccion { get; set; }
        public int NumeroMesa { get; set; }
        public bool Estado { get; set; }
    }
}
