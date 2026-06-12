using System;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class PersonaNegocio
    {
        public Persona ObtenerResponsablePorClasificacion(int idClasificacion)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT TOP 1 p.IdPersona, p.Nombre, p.Apellido, p.Email, p.Telefono, p.IdClasificacion " +
                        "FROM Personas p " +
                        "WHERE p.IdClasificacion = @IdClasificacion " +
                        "AND NOT EXISTS (SELECT 1 FROM Usuarios u WHERE u.IdPersona = p.IdPersona)");
                    datos.setearParametro("@IdClasificacion", idClasificacion);
                    datos.ejecutarLectura();

                    if (datos.Lector.Read())
                    {
                        return Mapear(datos.Lector);
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "obtener responsable del sector");
            }
        }

        private static Persona Mapear(SqlDataReader reader)
        {
            return new Persona
            {
                IdPersona = (int)reader["IdPersona"],
                Nombre = (string)reader["Nombre"],
                Apellido = (string)reader["Apellido"],
                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null,
                Telefono = reader["Telefono"] != DBNull.Value ? (string)reader["Telefono"] : null,
                IdClasificacion = reader["IdClasificacion"] != DBNull.Value ? (int?)reader["IdClasificacion"] : null
            };
        }
    }
}
