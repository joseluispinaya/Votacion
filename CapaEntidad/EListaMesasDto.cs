using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EListaMesasDto
    {
        public int IdDelegado { get; set; }
        public int IdEleccion { get; set; }
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string NombreRecinto { get; set; }
        public string NombreLocalidad { get; set; }
        public string NombreEleccion { get; set; }
        public string NroMesaStr => $"Nro: {NumeroMesa:D3}";
    }
}
