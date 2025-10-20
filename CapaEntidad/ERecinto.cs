using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ERecinto
    {
        public int IdRecinto { get; set; }
        public int IdLocalidad { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
