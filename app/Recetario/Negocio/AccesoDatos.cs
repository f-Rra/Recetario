using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    internal class AccesoDatos : IDisposable
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        private string ruta;
        private bool disposed = false;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            ruta = ConfigurationManager.ConnectionStrings["RecetarioDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(ruta))
            {
                throw new Exception("No se encontró la cadena de conexión 'RecetarioDB' en App.config");
            }

            conexion = new SqlConnection(ruta);
            comando = new SqlCommand();
            comando.CommandTimeout = 120;
        }

        public void setearConsulta(string consulta)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
        }

        public void setearProcedimiento(string sp)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = sp;
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void ejecutarLectura()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
                throw;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            finally
            {
                conexion.Close();
                comando.Parameters.Clear();
            }
        }

        public int ejecutarAccionReturn()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                var result = comando.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return 0;
                return Convert.ToInt32(result);
            }
            finally
            {
                conexion.Close();
                comando.Parameters.Clear();
            }
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (lector != null && !lector.IsClosed)
                    {
                        lector.Close();
                        lector.Dispose();
                    }

                    if (comando != null)
                    {
                        comando.Dispose();
                    }

                    if (conexion != null && conexion.State != ConnectionState.Closed)
                    {
                        conexion.Close();
                        conexion.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~AccesoDatos()
        {
            Dispose(false);
        }
    }
}
