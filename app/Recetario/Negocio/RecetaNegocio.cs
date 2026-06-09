using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class RecetaNegocio
    {
        public List<Receta> Listar()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT r.IdReceta, r.Codigo, r.Nombre, r.IdClasificacion, " +
                        "c.Nombre AS NombreClasificacion, r.PorcionesBase, r.Activo, r.Imagen " +
                        "FROM Recetas r " +
                        "INNER JOIN Clasificaciones c ON c.IdClasificacion = r.IdClasificacion " +
                        "WHERE r.Activo = 1 " +
                        "ORDER BY c.Nombre, r.Nombre");
                    datos.ejecutarLectura();

                    return MapearLista(datos.Lector);
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar recetas");
            }
        }

        public List<Receta> ListarPorClasificacion(int idClasificacion)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT r.IdReceta, r.Codigo, r.Nombre, r.IdClasificacion, " +
                        "c.Nombre AS NombreClasificacion, r.PorcionesBase, r.Activo, r.Imagen " +
                        "FROM Recetas r " +
                        "INNER JOIN Clasificaciones c ON c.IdClasificacion = r.IdClasificacion " +
                        "WHERE r.Activo = 1 AND r.IdClasificacion = @IdClasificacion " +
                        "ORDER BY r.Nombre");
                    datos.setearParametro("@IdClasificacion", idClasificacion);
                    datos.ejecutarLectura();

                    return MapearLista(datos.Lector);
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar recetas por clasificación");
            }
        }

        public Receta BuscarPorId(int idReceta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT r.IdReceta, r.Codigo, r.Nombre, r.IdClasificacion, " +
                        "c.Nombre AS NombreClasificacion, r.PorcionesBase, r.Activo, r.Imagen " +
                        "FROM Recetas r " +
                        "INNER JOIN Clasificaciones c ON c.IdClasificacion = r.IdClasificacion " +
                        "WHERE r.IdReceta = @IdReceta");
                    datos.setearParametro("@IdReceta", idReceta);
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
                throw NegocioException.FromDbException(ex, "buscar receta por ID");
            }
        }

        private static List<Receta> MapearLista(SqlDataReader reader)
        {
            List<Receta> recetas = new List<Receta>();
            while (reader.Read())
            {
                recetas.Add(Mapear(reader));
            }
            return recetas;
        }

        private static Receta Mapear(SqlDataReader reader)
        {
            return new Receta
            {
                IdReceta = (int)reader["IdReceta"],
                Codigo = (string)reader["Codigo"],
                Nombre = (string)reader["Nombre"],
                IdClasificacion = (int)reader["IdClasificacion"],
                NombreClasificacion = (string)reader["NombreClasificacion"],
                PorcionesBase = (int)reader["PorcionesBase"],
                Activo = (bool)reader["Activo"],
                Imagen = reader["Imagen"] != System.DBNull.Value ? (string)reader["Imagen"] : null
            };
        }
    }
}
