using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ClasificacionNegocio
    {
        public List<Clasificacion> Listar()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("SELECT IdClasificacion, Nombre FROM Clasificaciones ORDER BY Nombre");
                    datos.ejecutarLectura();

                    List<Clasificacion> clasificaciones = new List<Clasificacion>();
                    while (datos.Lector.Read())
                    {
                        clasificaciones.Add(new Clasificacion
                        {
                            IdClasificacion = (int)datos.Lector["IdClasificacion"],
                            Nombre = (string)datos.Lector["Nombre"]
                        });
                    }
                    return clasificaciones;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar clasificaciones");
            }
        }
    }
}
