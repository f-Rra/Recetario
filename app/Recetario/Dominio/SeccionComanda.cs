using System.Collections.Generic;

namespace Dominio
{
    public class SeccionComanda
    {
        public string NombreReceta { get; set; }
        public string Sector { get; set; }
        public string Responsable { get; set; }
        public List<IngredienteReceta> Ingredientes { get; set; }
        public List<Procedimiento> Procedimientos { get; set; }
        public List<Modificacion> Modificaciones { get; set; }
    }
}
