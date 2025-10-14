using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_articulo.Models
{
    public class ArticuloDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
       
    }
}