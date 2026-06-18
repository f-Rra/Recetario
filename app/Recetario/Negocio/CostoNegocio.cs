using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class CostoNegocio
    {
        public Costo CalcularCosto(int idReceta, int porciones, int idUsuario)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearProcedimiento("sp_CalcularCostoReceta");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.setearParametro("@Porciones", porciones);
                    datos.setearParametro("@IdUsuario", idUsuario);
                    datos.ejecutarLectura();

                    if (datos.Lector.Read())
                    {
                        return new Costo
                        {
                            IdCosto = (int)datos.Lector["IdCosto"],
                            IdReceta = idReceta,
                            Porciones = porciones,
                            IdUsuario = idUsuario,
                            CostoTotal = (decimal)datos.Lector["CostoTotal"],
                            CostoUnitario = (decimal)datos.Lector["CostoUnitario"]
                        };
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "calcular costo de receta");
            }
        }

        public List<DetalleCosto> ObtenerDetalle(int idReceta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT i.Descripcion AS NombreIngrediente, ir.CantBruta, u.Abreviatura AS Unidad, " +
                        "pv.Precio AS CostoUnitario, ir.CantBruta * pv.Precio AS CostoIngrediente " +
                        "FROM IngredientesxRecetas ir " +
                        "INNER JOIN Ingredientes i ON i.IdIngrediente = ir.IdIngrediente " +
                        "INNER JOIN Unidades u ON u.IdUnidad = ir.IdUnidad " +
                        "INNER JOIN PrecioxIngrediente pv ON pv.IdIngrediente = ir.IdIngrediente " +
                        "AND pv.FechaVigencia = (SELECT MAX(p2.FechaVigencia) FROM PrecioxIngrediente p2 WHERE p2.IdIngrediente = ir.IdIngrediente) " +
                        "WHERE ir.IdReceta = @IdReceta " +
                        "ORDER BY i.Descripcion");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.ejecutarLectura();

                    List<DetalleCosto> detalle = new List<DetalleCosto>();
                    while (datos.Lector.Read())
                    {
                        detalle.Add(new DetalleCosto
                        {
                            NombreIngrediente = (string)datos.Lector["NombreIngrediente"],
                            CantBruta = (decimal)datos.Lector["CantBruta"],
                            Unidad = (string)datos.Lector["Unidad"],
                            CostoUnitario = (decimal)datos.Lector["CostoUnitario"],
                            CostoIngrediente = (decimal)datos.Lector["CostoIngrediente"]
                        });
                    }
                    return detalle;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "obtener detalle de costo");
            }
        }

        public List<Costo> ListarHistorial()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT co.IdCosto, co.IdReceta, r.Nombre AS NombreReceta, co.Fecha, " +
                        "co.Porciones, co.CostoTotal, co.CostoUnitario, co.IdUsuario " +
                        "FROM Costos co " +
                        "INNER JOIN Recetas r ON r.IdReceta = co.IdReceta " +
                        "ORDER BY co.Fecha DESC");
                    datos.ejecutarLectura();

                    List<Costo> costos = new List<Costo>();
                    while (datos.Lector.Read())
                    {
                        costos.Add(new Costo
                        {
                            IdCosto = (int)datos.Lector["IdCosto"],
                            IdReceta = (int)datos.Lector["IdReceta"],
                            NombreReceta = (string)datos.Lector["NombreReceta"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Porciones = (int)datos.Lector["Porciones"],
                            CostoTotal = (decimal)datos.Lector["CostoTotal"],
                            CostoUnitario = (decimal)datos.Lector["CostoUnitario"],
                            IdUsuario = (int)datos.Lector["IdUsuario"]
                        });
                    }
                    return costos;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar historial de costos");
            }
        }
    }
}
