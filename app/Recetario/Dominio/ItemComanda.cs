using System.Collections.Generic;

namespace Dominio
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
