using System.Collections.Generic;
using Dominio;

namespace Presentacion.UserControls
{
    public class ItemComanda
    {
        public Receta Receta { get; set; }
        public List<Modificacion> Modificaciones { get; set; } = new List<Modificacion>();

        public string NombreReceta => Receta != null ? Receta.Nombre : "";
        public string Clasificacion => Receta != null ? Receta.NombreClasificacion : "";
        public int CantidadModificaciones => Modificaciones.Count;
    }
}
