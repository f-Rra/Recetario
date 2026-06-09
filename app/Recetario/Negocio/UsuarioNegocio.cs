using System;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public Usuario ValidarUsuario(string email, string password)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearProcedimiento("sp_ValidarUsuario");
                    datos.setearParametro("@Email", email);
                    datos.setearParametro("@Password", password);
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
                throw NegocioException.FromDbException(ex, "validar usuario");
            }
        }

        private static Usuario Mapear(SqlDataReader reader)
        {
            Usuario usuario = new Usuario
            {
                IdUsuario = (int)reader["IdUsuario"],
                IdPersona = (int)reader["IdPersona"],
                Rol = (string)reader["Rol"]
            };

            usuario.Persona = new Persona
            {
                IdPersona = (int)reader["IdPersona"],
                Nombre = (string)reader["Nombre"],
                Apellido = (string)reader["Apellido"],
                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null,
                IdClasificacion = reader["IdClasificacion"] != DBNull.Value ? (int?)reader["IdClasificacion"] : null,
                NombreClasificacion = reader["NombreClasificacion"] != DBNull.Value ? (string)reader["NombreClasificacion"] : null
            };

            return usuario;
        }
    }
}
