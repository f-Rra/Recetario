using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class StockNegocio
    {
        public List<Ingrediente> ListarStockCritico()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT Codigo, Descripcion, StockActual, StockMinimo, Unidad FROM vw_StockCritico");
                    datos.ejecutarLectura();

                    List<Ingrediente> ingredientes = new List<Ingrediente>();
                    while (datos.Lector.Read())
                    {
                        ingredientes.Add(new Ingrediente
                        {
                            Codigo = (string)datos.Lector["Codigo"],
                            Descripcion = (string)datos.Lector["Descripcion"],
                            StockActual = (decimal)datos.Lector["StockActual"],
                            StockMinimo = (decimal)datos.Lector["StockMinimo"],
                            Abreviatura = (string)datos.Lector["Unidad"]
                        });
                    }
                    return ingredientes;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar stock crítico");
            }
        }

        public void RegistrarMovimiento(MovimientoStock movimiento)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO MovimientosStock " +
                        "(IdIngrediente, IdTipoMovimiento, Cantidad, IdUnidad, IdUsuario, Observaciones) " +
                        "VALUES (@IdIngrediente, @IdTipoMovimiento, @Cantidad, @IdUnidad, @IdUsuario, @Observaciones)");
                    datos.setearParametro("@IdIngrediente", movimiento.IdIngrediente);
                    datos.setearParametro("@IdTipoMovimiento", movimiento.IdTipoMovimiento);
                    datos.setearParametro("@Cantidad", movimiento.Cantidad);
                    datos.setearParametro("@IdUnidad", movimiento.IdUnidad);
                    datos.setearParametro("@IdUsuario", movimiento.IdUsuario);
                    datos.setearParametro("@Observaciones", movimiento.Observaciones);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "registrar movimiento de stock");
            }
        }

        public List<MovimientoStock> ListarMovimientos()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT m.IdMovimiento, m.IdIngrediente, i.Descripcion AS NombreIngrediente, " +
                        "m.IdTipoMovimiento, tm.Nombre AS TipoMovimiento, m.Cantidad, " +
                        "m.IdUnidad, u.Abreviatura, m.Fecha, m.IdUsuario, m.Observaciones " +
                        "FROM MovimientosStock m " +
                        "INNER JOIN Ingredientes i ON i.IdIngrediente = m.IdIngrediente " +
                        "INNER JOIN TiposMovimiento tm ON tm.IdTipoMovimiento = m.IdTipoMovimiento " +
                        "INNER JOIN Unidades u ON u.IdUnidad = m.IdUnidad " +
                        "ORDER BY m.Fecha DESC");
                    datos.ejecutarLectura();

                    List<MovimientoStock> movimientos = new List<MovimientoStock>();
                    while (datos.Lector.Read())
                    {
                        movimientos.Add(new MovimientoStock
                        {
                            IdMovimiento = (int)datos.Lector["IdMovimiento"],
                            IdIngrediente = (int)datos.Lector["IdIngrediente"],
                            NombreIngrediente = (string)datos.Lector["NombreIngrediente"],
                            IdTipoMovimiento = (int)datos.Lector["IdTipoMovimiento"],
                            TipoMovimiento = (string)datos.Lector["TipoMovimiento"],
                            Cantidad = (decimal)datos.Lector["Cantidad"],
                            IdUnidad = (int)datos.Lector["IdUnidad"],
                            Abreviatura = (string)datos.Lector["Abreviatura"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            IdUsuario = (int)datos.Lector["IdUsuario"],
                            Observaciones = datos.Lector["Observaciones"] != DBNull.Value ? (string)datos.Lector["Observaciones"] : null
                        });
                    }
                    return movimientos;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar movimientos de stock");
            }
        }

        public List<TipoMovimiento> ListarTipos()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("SELECT IdTipoMovimiento, Nombre FROM TiposMovimiento ORDER BY Nombre");
                    datos.ejecutarLectura();

                    List<TipoMovimiento> tipos = new List<TipoMovimiento>();
                    while (datos.Lector.Read())
                    {
                        tipos.Add(new TipoMovimiento
                        {
                            IdTipoMovimiento = (int)datos.Lector["IdTipoMovimiento"],
                            Nombre = (string)datos.Lector["Nombre"]
                        });
                    }
                    return tipos;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar tipos de movimiento");
            }
        }
    }
}
