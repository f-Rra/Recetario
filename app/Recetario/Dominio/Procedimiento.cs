using System.ComponentModel;

namespace Dominio
{
    // Paso del procedimiento de preparación de una receta.
    public class Procedimiento
    {
        public int IdProcedimiento { get; set; }

        public int IdReceta { get; set; }

        [DisplayName("Paso")]
        public int NroPaso { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
    }
}
