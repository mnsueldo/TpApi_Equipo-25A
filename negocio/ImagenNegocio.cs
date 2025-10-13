using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using Negocio;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Negocio
{
    public class ImagenNegocio
    {
        public List<Imagen> listar(int idArticulo)
        {

            List<Imagen> lista = new List<Imagen>();
            AccesoDatos Datos = new AccesoDatos();

            try
            {
                Datos.setearConsulta("SELECT Id,IdArticulo,ImagenUrl FROM IMAGENES WHERE IdArticulo = @idArticulo");
                Datos.setearParametro("@idArticulo", idArticulo);
                Datos.ejecutarLectura();

                while (Datos.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.IdArticulo = (int)Datos.Lector["IdArticulo"];
                    aux.Id = (int)Datos.Lector["Id"];
                    aux.ImagenUrl = (string)Datos.Lector["ImagenUrl"];

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

        public void agregar(Imagen nuevo)
        {
            AccesoDatos Datos = new AccesoDatos();

            try
            {

                Datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) " + "VALUES (@IdArticulo, @ImagenUrl)");

                Datos.setearParametro("@IdArticulo", nuevo.IdArticulo);
                Datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);

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
        
    }
}