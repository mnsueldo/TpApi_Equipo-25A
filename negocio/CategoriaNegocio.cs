using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> Lista()
        {

            List<Categoria> lista = new List<Categoria>();
            AccesoDatos Datos = new AccesoDatos();

            try
            {
                Datos.setearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
                Datos.ejecutarLectura();

                while (Datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
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

        public void agregar(Categoria nuevo)
        {
            AccesoDatos Datos = new AccesoDatos();

            try
            {

                Datos.setearConsulta("INSERT INTO CATEGORIAS (Descripcion) " + "VALUES (@descripcion)");
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

        public void modificar(Categoria modificar)
        {
            AccesoDatos Datos = new AccesoDatos();

            try
            {
                Datos.setearConsulta("UPDATE CATEGORIAS SET Descripcion = @descripcion WHERE Id = @id");


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
                Datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @id");
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
