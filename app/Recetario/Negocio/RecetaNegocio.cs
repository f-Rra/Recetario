using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class RecetaNegocio
    {
        #region Consultas

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

        public string ObtenerProximoCodigo()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(Codigo, 4, 10) AS INT)), 0) + 1 FROM Recetas WHERE Codigo LIKE 'REC%'");
                    int numero = datos.ejecutarAccionReturn();
                    return "REC" + numero.ToString("D3");
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "generar código de receta");
            }
        }

        public List<IngredienteReceta> ListarIngredientesComanda(int idReceta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT NombreIngrediente, Cantidad AS CantNeta, Unidad AS Abreviatura " +
                        "FROM vw_Comanda WHERE IdReceta = @IdReceta ORDER BY NombreIngrediente");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.ejecutarLectura();

                    List<IngredienteReceta> ingredientes = new List<IngredienteReceta>();
                    while (datos.Lector.Read())
                    {
                        ingredientes.Add(new IngredienteReceta
                        {
                            NombreIngrediente = (string)datos.Lector["NombreIngrediente"],
                            CantNeta          = (decimal)datos.Lector["CantNeta"],
                            Abreviatura       = (string)datos.Lector["Abreviatura"]
                        });
                    }
                    return ingredientes;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar ingredientes de la comanda");
            }
        }

        public List<IngredienteReceta> ListarIngredientes(int idReceta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT ir.IdReceta, ir.IdIngrediente, i.Descripcion AS NombreIngrediente, " +
                        "ir.CantNeta, ir.Rendimiento, ir.CantBruta, ir.IdUnidad, u.Abreviatura " +
                        "FROM IngredientesxRecetas ir " +
                        "INNER JOIN Ingredientes i ON i.IdIngrediente = ir.IdIngrediente " +
                        "INNER JOIN Unidades u ON u.IdUnidad = ir.IdUnidad " +
                        "WHERE ir.IdReceta = @IdReceta " +
                        "ORDER BY i.Descripcion");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.ejecutarLectura();

                    List<IngredienteReceta> ingredientes = new List<IngredienteReceta>();
                    while (datos.Lector.Read())
                    {
                        ingredientes.Add(new IngredienteReceta
                        {
                            IdReceta = (int)datos.Lector["IdReceta"],
                            IdIngrediente = (int)datos.Lector["IdIngrediente"],
                            NombreIngrediente = (string)datos.Lector["NombreIngrediente"],
                            CantNeta = (decimal)datos.Lector["CantNeta"],
                            Rendimiento = (decimal)datos.Lector["Rendimiento"],
                            CantBruta = (decimal)datos.Lector["CantBruta"],
                            IdUnidad = (int)datos.Lector["IdUnidad"],
                            Abreviatura = (string)datos.Lector["Abreviatura"]
                        });
                    }
                    return ingredientes;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar ingredientes de la receta");
            }
        }

        #endregion

        #region Modificaciones

        public int Agregar(Receta receta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO Recetas (Codigo, Nombre, IdClasificacion, PorcionesBase, Activo, Imagen) " +
                        "VALUES (@Codigo, @Nombre, @IdClasificacion, @PorcionesBase, 1, @Imagen); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);");
                    datos.setearParametro("@Codigo", receta.Codigo);
                    datos.setearParametro("@Nombre", receta.Nombre);
                    datos.setearParametro("@IdClasificacion", receta.IdClasificacion);
                    datos.setearParametro("@PorcionesBase", receta.PorcionesBase);
                    datos.setearParametro("@Imagen", receta.Imagen);

                    return datos.ejecutarAccionReturn();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "agregar receta");
            }
        }

        public void Modificar(Receta receta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "UPDATE Recetas SET Codigo = @Codigo, Nombre = @Nombre, " +
                        "IdClasificacion = @IdClasificacion, PorcionesBase = @PorcionesBase " +
                        "WHERE IdReceta = @IdReceta");
                    datos.setearParametro("@IdReceta", receta.IdReceta);
                    datos.setearParametro("@Codigo", receta.Codigo);
                    datos.setearParametro("@Nombre", receta.Nombre);
                    datos.setearParametro("@IdClasificacion", receta.IdClasificacion);
                    datos.setearParametro("@PorcionesBase", receta.PorcionesBase);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "modificar receta");
            }
        }

        public void Eliminar(int idReceta)
        {
            // Baja lógica: la receta puede estar referenciada por comandas y costos.
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("UPDATE Recetas SET Activo = 0 WHERE IdReceta = @IdReceta");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "eliminar receta");
            }
        }

        #endregion

        #region Ingredientes de receta

        public void AgregarIngrediente(IngredienteReceta ingredienteReceta)
        {
            // CantBruta = CantNeta / (Rendimiento / 100): contempla la merma.
            decimal cantBruta = ingredienteReceta.Rendimiento > 0
                ? ingredienteReceta.CantNeta / (ingredienteReceta.Rendimiento / 100m)
                : ingredienteReceta.CantNeta;

            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO IngredientesxRecetas (IdReceta, IdIngrediente, CantNeta, Rendimiento, CantBruta, IdUnidad) " +
                        "VALUES (@IdReceta, @IdIngrediente, @CantNeta, @Rendimiento, @CantBruta, @IdUnidad)");
                    datos.setearParametro("@IdReceta", ingredienteReceta.IdReceta);
                    datos.setearParametro("@IdIngrediente", ingredienteReceta.IdIngrediente);
                    datos.setearParametro("@CantNeta", ingredienteReceta.CantNeta);
                    datos.setearParametro("@Rendimiento", ingredienteReceta.Rendimiento);
                    datos.setearParametro("@CantBruta", cantBruta);
                    datos.setearParametro("@IdUnidad", ingredienteReceta.IdUnidad);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "agregar ingrediente a la receta");
            }
        }

        public void QuitarIngrediente(int idReceta, int idIngrediente)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("DELETE FROM IngredientesxRecetas WHERE IdReceta = @IdReceta AND IdIngrediente = @IdIngrediente");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.setearParametro("@IdIngrediente", idIngrediente);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "quitar ingrediente de la receta");
            }
        }

        #endregion

        #region Mapeo

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

        #endregion
    }
}
