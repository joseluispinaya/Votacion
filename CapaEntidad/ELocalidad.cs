using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ELocalidad
    {
        public int IdLocalidad { get; set; }
        public string Nombre { get; set; }
        public List<ERecinto> ListaRecintos { get; set; }
        public string NumRecintos =>
            ListaRecintos == null || ListaRecintos.Count == 0
                ? "0 Recintos"
                : ListaRecintos.Count == 1
                    ? "1 Recinto"
                    : $"{ListaRecintos.Count} Recintos";


        //public int NumRecintos => ListaRecintos == null ? 0 : ListaRecintos.Count;
    }
}
