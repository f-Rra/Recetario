using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class UnidadNegocio
    {
        public List<Unidad> Listar()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("SELECT IdUnidad, Nombre, Abreviatura FROM Unidades ORDER BY Nombre");
                    datos.ejecutarLectura();

                    List<Unidad> unidades = new List<Unidad>();
                    while (datos.Lector.Read())
                    {
                        unidades.Add(new Unidad
                        {
                            IdUnidad = (int)datos.Lector["IdUnidad"],
                            Nombre = (string)datos.Lector["Nombre"],
                            Abreviatura = (string)datos.Lector["Abreviatura"]
                        });
                    }
                    return unidades;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar unidades");
            }
        }
    }
}
