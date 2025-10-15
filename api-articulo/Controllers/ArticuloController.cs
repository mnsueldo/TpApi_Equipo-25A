using api_articulo.Models;
using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace api_articulo.Controllers
{
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public HttpResponseMessage Get()
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                List<Articulo> lista = negocio.listar();

                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }

        // GET: api/Articulo/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                if (id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ID inválido.");

                ArticuloNegocio negocio = new ArticuloNegocio();
                List<Articulo> lista = negocio.listar();

                Articulo articulo = lista.Find(x => x.Id == id);               

                if (articulo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Artículo no encontrado.");

                return Request.CreateResponse(HttpStatusCode.OK, articulo);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // POST: api/Articulo
        public HttpResponseMessage Post([FromBody] ArticuloDto articulo)
        {

            try
            {

                ArticuloNegocio negocio = new ArticuloNegocio();

                if (articulo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Los campos no pueden ser null o estar vacíos.");

                if (string.IsNullOrWhiteSpace(articulo.Codigo))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El código es obligatorio.");

                if (string.IsNullOrWhiteSpace(articulo.Nombre))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El nombre es obligatorio.");

                if (articulo.Descripcion.Length > 500)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La descripción no puede superar los 500 caracteres.");

                if (articulo.Precio <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio debe ser mayor a 0.");

                MarcaNegocio marcaNegocio = new MarcaNegocio();
                Marca marca = marcaNegocio.listar().Find(x => x.Id == articulo.IdMarca);

                if (marca == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");

                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                Categoria categoria = categoriaNegocio.listar().Find(x => x.Id == articulo.IdCategoria);

                if (categoria == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La Categoria no existe.");

                if (negocio.ExisteCodigo(articulo.Codigo))
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Ya existe un artículo con el código: " + articulo.Codigo);

                var nuevo = new Articulo
                {
                    Codigo = articulo.Codigo,
                    Nombre = articulo.Nombre,
                    Descripcion = articulo.Descripcion,
                    Precio = articulo.Precio,
                    Marca = new Marca { Id = articulo.IdMarca },
                    Categoria = new Categoria { Id = articulo.IdCategoria },

                };

                negocio.agregar(nuevo);

                return Request.CreateResponse(HttpStatusCode.OK, "Articulo agregado correctamente.");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }


        }
        // POST: api/Articulo/5/imagenes
        //POST para agregar imagenes al prducto

        [Route("api/Articulo/{idArticulo}/imagenes")]
        public HttpResponseMessage Post(int idArticulo, [FromBody] ImagenDto url)
        {
            try
            {
                ImagenNegocio negocio = new ImagenNegocio();

                if (idArticulo <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El ID del artículo no es válido.");

                ArticuloNegocio articuloNegocio = new ArticuloNegocio();

                if (!articuloNegocio.ExisteId(idArticulo))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No existe un artículo con el ID: " + idArticulo);
                                
                if (url == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El campo no puede estar vacio o ser null");

                
                var nuevaImagen = new Imagen
                {
                    IdArticulo = idArticulo,
                    ImagenUrl = url.ToString()

                };

                
                negocio.agregar(nuevaImagen);
                                

                return Request.CreateResponse(HttpStatusCode.Created, "Imagen agregada correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado al subir la imagen.");
            }
        }

        // PUT: api/Articulo/5

        public HttpResponseMessage Put(int id, [FromBody] ArticuloDto articulo)
        {

            try
            {

                ArticuloNegocio negocio = new ArticuloNegocio();

                if (articulo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El cuerpo de la solicitud está vacío.");

                if (string.IsNullOrWhiteSpace(articulo.Codigo))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El código es obligatorio.");

                if (string.IsNullOrWhiteSpace(articulo.Nombre))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El nombre es obligatorio.");

                if (articulo.Descripcion.Length > 500)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La descripción no puede superar los 500 caracteres.");

                if (articulo.Precio <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio debe ser mayor a 0.");

                MarcaNegocio marcaNegocio = new MarcaNegocio();
                Marca marca = marcaNegocio.listar().Find(x => x.Id == articulo.IdMarca);

                if (marca == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");

                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                Categoria categoria = categoriaNegocio.listar().Find(x => x.Id == articulo.IdCategoria);

                if (categoria == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La Categoria no existe.");

                if (!negocio.ExisteId(id))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Artículo no encontrado.");

                var modificado = new Articulo
                {
                    Codigo = articulo.Codigo,
                    Nombre = articulo.Nombre,
                    Descripcion = articulo.Descripcion,
                    Precio = articulo.Precio,
                    Marca = new Marca { Id = articulo.IdMarca },
                    Categoria = new Categoria { Id = articulo.IdCategoria },
                    Id = id

                };

                negocio.modificar(modificado);

                return Request.CreateResponse(HttpStatusCode.OK, "Articulo modificado correctamente.");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }
        }
        // DELETE: api/Articulo/5
        
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ID inválido.");

                ArticuloNegocio negocio = new ArticuloNegocio();

                if (!negocio.ExisteId(id))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Artículo no encontrado.");

                negocio.eliminar(id);

                return Request.CreateResponse(HttpStatusCode.OK, "Artículo eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
