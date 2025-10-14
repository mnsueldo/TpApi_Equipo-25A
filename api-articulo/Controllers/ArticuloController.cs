using api_articulo.Models;
using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                if (negocio.ExisteCodigo(articulo.Codigo))
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Ya existe un artículo con el código" + ": "+ articulo.Codigo);

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

        // PUT: api/Articulo/5
        public void Put(int id, [FromBody]ArticuloDto articulo)
        {
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
        }
    }
}
