using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;

namespace Negocio
{
    public class MarcaNegocio
    {
        public List<Marca> Lista()
        {

            List<Marca> lista = new List<Marca>();
            AccesoDatos Datos = new AccesoDatos();

            try
            {
                Datos.setearConsulta("SELECT Id, Descripcion FROM MARCAS");
                Datos.ejecutarLectura();

                while (Datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)Datos.Lector["Id"];
                    aux.Descripcion = (string)Datos.Lector["Descripcion"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Datos.cerrarConexion();
            }
        }

        public void agregar(Marca nuevo)
        {
            AccesoDatos Datos = new AccesoDatos();

            try
            {

                Datos.setearConsulta("INSERT INTO MARCAS (Descripcion) " + "VALUES (@descripcion)");
                Datos.setearParametro("@descripcion", nuevo.Descripcion);

                Datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Datos.cerrarConexion();

            }
        }

        public void modificar(Marca modificar)
        {
            AccesoDatos Datos = new AccesoDatos();

            try
            {
                Datos.setearConsulta("UPDATE MARCAS SET Descripcion = @descripcion WHERE Id = @id");


                Datos.setearParametro("@id", modificar.Id);
                Datos.setearParametro("@descripcion", modificar.Descripcion);


                Datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void eliminar(int id)
        {
            AccesoDatos Datos = new AccesoDatos();
            try
            {
                Datos.setearConsulta("DELETE FROM MARCAS WHERE Id = @id");
                Datos.setearParametro("@id", id);
                Datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }


}
