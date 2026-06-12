using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ProcedimientoNegocio
    {
        public List<Procedimiento> ListarPorReceta(int idReceta)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT IdProcedimiento, IdReceta, NroPaso, Descripcion " +
                        "FROM Procedimientos WHERE IdReceta = @IdReceta ORDER BY NroPaso");
                    datos.setearParametro("@IdReceta", idReceta);
                    datos.ejecutarLectura();

                    List<Procedimiento> pasos = new List<Procedimiento>();
                    while (datos.Lector.Read())
                    {
                        pasos.Add(new Procedimiento
                        {
                            IdProcedimiento = (int)datos.Lector["IdProcedimiento"],
                            IdReceta = (int)datos.Lector["IdReceta"],
                            NroPaso = (int)datos.Lector["NroPaso"],
                            Descripcion = (string)datos.Lector["Descripcion"]
                        });
                    }
                    return pasos;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar procedimientos");
            }
        }
    }
}
