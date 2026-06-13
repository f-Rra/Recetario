using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ComandaNegocio
    {
        public int RegistrarComanda(int idReceta, int porciones, int idUsuario, int? idPersona = null)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearProcedimiento("sp_RegistrarComanda");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.setearParametro("@Porciones", porciones);
                    datos.setearParametro("@IdUsuario", idUsuario);
                    datos.setearParametro("@IdPersona", idPersona);

                    return datos.ejecutarAccionReturn();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "registrar comanda");
            }
        }

        public List<IngredienteReceta> AjustarReceta(int idReceta, int comensales)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearProcedimiento("sp_AjustarReceta");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.setearParametro("@Comensales", comensales);
                    datos.ejecutarLectura();

                    List<IngredienteReceta> ingredientes = new List<IngredienteReceta>();
                    while (datos.Lector.Read())
                    {
                        ingredientes.Add(new IngredienteReceta
                        {
                            IdReceta = idReceta,
                            IdIngrediente = (int)datos.Lector["IdIngrediente"],
                            NombreIngrediente = (string)datos.Lector["NombreIngrediente"],
                            CantNeta = (decimal)datos.Lector["CantidadAjustada"],
                            Abreviatura = (string)datos.Lector["Unidad"]
                        });
                    }
                    return ingredientes;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "ajustar receta");
            }
        }

        public List<TipoModificacion> ListarTiposModificacion()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("SELECT IdTipoModificacion, Nombre FROM TiposModificacion ORDER BY IdTipoModificacion");
                    datos.ejecutarLectura();

                    List<TipoModificacion> tipos = new List<TipoModificacion>();
                    while (datos.Lector.Read())
                    {
                        tipos.Add(new TipoModificacion
                        {
                            IdTipoModificacion = (int)datos.Lector["IdTipoModificacion"],
                            Nombre = (string)datos.Lector["Nombre"]
                        });
                    }
                    return tipos;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar tipos de modificación");
            }
        }

        public void RegistrarModificacion(Modificacion modificacion)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO Modificaciones " +
                        "(IdComanda, IdTipoModificacion, IdIngredienteOriginal, IdIngredienteReemplazo, Cantidad, IdUnidad) " +
                        "VALUES (@IdComanda, @IdTipoModificacion, @IdIngredienteOriginal, @IdIngredienteReemplazo, @Cantidad, @IdUnidad)");
                    datos.setearParametro("@IdComanda", modificacion.IdComanda);
                    datos.setearParametro("@IdTipoModificacion", modificacion.IdTipoModificacion);
                    datos.setearParametro("@IdIngredienteOriginal", modificacion.IdIngredienteOriginal);
                    datos.setearParametro("@IdIngredienteReemplazo", modificacion.IdIngredienteReemplazo);
                    datos.setearParametro("@Cantidad", modificacion.Cantidad);
                    datos.setearParametro("@IdUnidad", modificacion.IdUnidad);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "registrar modificación");
            }
        }
    }
}
