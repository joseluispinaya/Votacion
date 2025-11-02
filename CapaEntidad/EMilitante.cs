using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EMilitante
    {
        public int IdMilitante { get; set; }
        public string NombresCompleto { get; set; }
        public string NroCI { get; set; }
        public string FechaNacido { get; set; }
        public DateTime VFechaNacido { get; set; }
        public string Celular { get; set; }
        public int IdDistrito { get; set; }
        public bool Estado { get; set; }
        public DateTime VFechaRegistro { get; set; }
        public string FechaRegistro { get; set; }
        public EDistrito RefDistrito { get; set; }
        public string Edad
        {
            get
            {
                var hoy = DateTime.Today;
                int edad = hoy.Year - VFechaNacido.Year;

                // Si aún no cumplió años este año, restamos 1
                if (VFechaNacido.Date > hoy.AddYears(-edad))
                    edad--;

                return $"{edad} años";
            }
        }
    }
}
