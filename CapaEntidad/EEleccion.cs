using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EEleccion
    {
        public int IdEleccion { get; set; }
        public int IdTipo { get; set; }
        public int IdAdmin { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }
        public ETipoEleccion RefTipoEleccion { get; set; }
    }
}
