using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureHolicBackend.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name must be added")]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; } //IFromFile is not a data type which can be recognised by the database
        public string ImageUrl { get; set; } // will be recognised by DB because it is of type string




    }
}
