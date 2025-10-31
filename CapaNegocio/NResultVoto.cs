﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class NResultVoto
    {
        #region "PATRON SINGLETON"
        private static NResultVoto daoEmpleado = null;
        private NResultVoto() { }
        public static NResultVoto GetInstance()
        {
            if (daoEmpleado == null)
            {
                daoEmpleado = new NResultVoto();
            }
            return daoEmpleado;
        }
        #endregion

        public Respuesta<bool> GuardarVotos(int IdEleccion, int IdMesa, int IdDelegado, int Nulos, int Blancos, List<EResultado> ListaResultados)
        {
            return DResultVoto.GetInstance().GuardarVotos(IdEleccion, IdMesa, IdDelegado, Nulos, Blancos, ListaResultados);
        }

        public Respuesta<bool> GuardarVotosNuevo(string VotosXml)
        {
            return DResultVoto.GetInstance().GuardarVotosNuevo(VotosXml);
        }

        public Respuesta<List<EResultadoGeneral>> ResultadoGeneralVotacion(int IdEleccion)
        {
            return DResultVoto.GetInstance().ResultadoGeneralVotacion(IdEleccion);
        }

    }
}
