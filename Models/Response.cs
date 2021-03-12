using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest_Geo.Models
{
    public class Response
    {
        public int id { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string estado { get; set; }

        public static Response Parse(Geo model) {
            var res = new Response();
            res.id = model.Id;
            res.latitud = model.Latitud;
            res.longitud = model.Longitud;
            switch (model.Estado)
            {
                case Geo.eEstados.Procesando:
                    res.estado = "Procesando";
                    break;
                case Geo.eEstados.Terminado:
                    res.estado = "Terminado";
                    break;
                case Geo.eEstados.ErrorOSM:
                    res.estado = "ErrorOSM";
                    break;
            }
            return res;
        }
    }
}
