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

        public void Agregar(Procedimiento procedimiento)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO Procedimientos (IdReceta, NroPaso, Descripcion) " +
                        "VALUES (@IdReceta, @NroPaso, @Descripcion)");
                    datos.setearParametro("@IdReceta", procedimiento.IdReceta);
                    datos.setearParametro("@NroPaso", procedimiento.NroPaso);
                    datos.setearParametro("@Descripcion", procedimiento.Descripcion);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "agregar paso del procedimiento");
            }
        }

        public void Eliminar(int idProcedimiento)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("DELETE FROM Procedimientos WHERE IdProcedimiento = @IdProcedimiento");
                    datos.setearParametro("@IdProcedimiento", idProcedimiento);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "eliminar paso del procedimiento");
            }
        }
    }
}
