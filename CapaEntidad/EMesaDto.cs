using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EMesaDto
    {
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public string NombreCompleto { get; set; }
        public string Celular { get; set; }
        public string NroMesaStr => $"Nro: {NumeroMesa:D3}";
    }
}
