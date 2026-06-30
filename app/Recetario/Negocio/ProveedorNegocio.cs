using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ProveedorNegocio
    {
        #region Consultas

        public List<Proveedor> Listar()
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT IdProveedor, Nombre, Contacto, Telefono, Email, Direccion " +
                        "FROM Proveedores ORDER BY Nombre");
                    datos.ejecutarLectura();

                    List<Proveedor> proveedores = new List<Proveedor>();
                    while (datos.Lector.Read())
                    {
                        proveedores.Add(Mapear(datos.Lector));
                    }
                    return proveedores;
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "listar proveedores");
            }
        }

        public Proveedor BuscarPorId(int idProveedor)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "SELECT IdProveedor, Nombre, Contacto, Telefono, Email, Direccion " +
                        "FROM Proveedores WHERE IdProveedor = @IdProveedor");
                    datos.setearParametro("@IdProveedor", idProveedor);
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
                throw NegocioException.FromDbException(ex, "buscar proveedor por ID");
            }
        }

        #endregion

        #region Modificaciones

        public void Agregar(Proveedor proveedor)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "INSERT INTO Proveedores (Nombre, Contacto, Telefono, Email, Direccion) " +
                        "VALUES (@Nombre, @Contacto, @Telefono, @Email, @Direccion)");
                    datos.setearParametro("@Nombre", proveedor.Nombre);
                    datos.setearParametro("@Contacto", proveedor.Contacto);
                    datos.setearParametro("@Telefono", proveedor.Telefono);
                    datos.setearParametro("@Email", proveedor.Email);
                    datos.setearParametro("@Direccion", proveedor.Direccion);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "agregar proveedor");
            }
        }

        public void Modificar(Proveedor proveedor)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta(
                        "UPDATE Proveedores SET Nombre = @Nombre, Contacto = @Contacto, " +
                        "Telefono = @Telefono, Email = @Email, Direccion = @Direccion " +
                        "WHERE IdProveedor = @IdProveedor");
                    datos.setearParametro("@IdProveedor", proveedor.IdProveedor);
                    datos.setearParametro("@Nombre", proveedor.Nombre);
                    datos.setearParametro("@Contacto", proveedor.Contacto);
                    datos.setearParametro("@Telefono", proveedor.Telefono);
                    datos.setearParametro("@Email", proveedor.Email);
                    datos.setearParametro("@Direccion", proveedor.Direccion);

                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "modificar proveedor");
            }
        }

        public void Eliminar(int idProveedor)
        {
            try
            {
                using (AccesoDatos datos = new AccesoDatos())
                {
                    datos.setearConsulta("DELETE FROM Proveedores WHERE IdProveedor = @IdProveedor");
                    datos.setearParametro("@IdProveedor", idProveedor);
                    datos.ejecutarAccion();
                }
            }
            catch (SqlException ex)
            {
                throw NegocioException.FromDbException(ex, "eliminar proveedor");
            }
        }

        #endregion

        #region Mapeo

        private static Proveedor Mapear(SqlDataReader reader)
        {
            return new Proveedor
            {
                IdProveedor = (int)reader["IdProveedor"],
                Nombre = (string)reader["Nombre"],
                Contacto = reader["Contacto"] != DBNull.Value ? (string)reader["Contacto"] : null,
                Telefono = reader["Telefono"] != DBNull.Value ? (string)reader["Telefono"] : null,
                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null,
                Direccion = reader["Direccion"] != DBNull.Value ? (string)reader["Direccion"] : null
            };
        }

        #endregion
    }
}
