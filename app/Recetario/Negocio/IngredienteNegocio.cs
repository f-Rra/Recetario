using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class IngredienteNegocio
    {
        public List<Ingrediente> Listar()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT i.IdIngrediente, i.Codigo, i.Descripcion, i.IdUnidad, " +
                        "u.Abreviatura, i.StockActual, i.StockMinimo " +
                        "FROM Ingredientes i " +
                        "INNER JOIN Unidades u ON u.IdUnidad = i.IdUnidad " +
                        "ORDER BY i.Descripcion");
                    datos.ejecutarLectura();

                    List<Ingrediente> ingredientes = new List<Ingrediente>();
                    while (datos.Lector.Read())
                    {
                        ingredientes.Add(Mapear(datos.Lector));
                    }
                    return ingredientes;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar ingredientes");
            }
        }

        public Ingrediente BuscarPorId(int idIngrediente)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT i.IdIngrediente, i.Codigo, i.Descripcion, i.IdUnidad, " +
                        "u.Abreviatura, i.StockActual, i.StockMinimo " +
                        "FROM Ingredientes i " +
                        "INNER JOIN Unidades u ON u.IdUnidad = i.IdUnidad " +
                        "WHERE i.IdIngrediente = @IdIngrediente");
                    datos.setearParametro("@IdIngrediente", idIngrediente);
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
                throw NegocioException.FromDbException(ex, "buscar ingrediente por ID");
            }
        }

        public void Agregar(Ingrediente ingrediente)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO Ingredientes (Codigo, Descripcion, IdUnidad, StockActual, StockMinimo) " +
                        "VALUES (@Codigo, @Descripcion, @IdUnidad, @StockActual, @StockMinimo)");
                    datos.setearParametro("@Codigo", ingrediente.Codigo);
                    datos.setearParametro("@Descripcion", ingrediente.Descripcion);
                    datos.setearParametro("@IdUnidad", ingrediente.IdUnidad);
                    datos.setearParametro("@StockActual", ingrediente.StockActual);
                    datos.setearParametro("@StockMinimo", ingrediente.StockMinimo);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "agregar ingrediente");
            }
        }

        public void Modificar(Ingrediente ingrediente)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "UPDATE Ingredientes SET Codigo = @Codigo, Descripcion = @Descripcion, " +
                        "IdUnidad = @IdUnidad, StockMinimo = @StockMinimo " +
                        "WHERE IdIngrediente = @IdIngrediente");
                    datos.setearParametro("@IdIngrediente", ingrediente.IdIngrediente);
                    datos.setearParametro("@Codigo", ingrediente.Codigo);
                    datos.setearParametro("@Descripcion", ingrediente.Descripcion);
                    datos.setearParametro("@IdUnidad", ingrediente.IdUnidad);
                    datos.setearParametro("@StockMinimo", ingrediente.StockMinimo);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "modificar ingrediente");
            }
        }

        public void Eliminar(int idIngrediente)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("DELETE FROM Ingredientes WHERE IdIngrediente = @IdIngrediente");
                    datos.setearParametro("@IdIngrediente", idIngrediente);
                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "eliminar ingrediente");
            }
        }

        private static Ingrediente Mapear(SqlDataReader reader)
        {
            return new Ingrediente
            {
                IdIngrediente = (int)reader["IdIngrediente"],
                Codigo = (string)reader["Codigo"],
                Descripcion = (string)reader["Descripcion"],
                IdUnidad = (int)reader["IdUnidad"],
                Abreviatura = (string)reader["Abreviatura"],
                StockActual = (decimal)reader["StockActual"],
                StockMinimo = (decimal)reader["StockMinimo"]
            };
        }
    }
}
