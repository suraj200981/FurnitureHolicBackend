using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureHolicBackend.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Email must be added")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password must be added")]
        public string Password { get; set; }

        public string Phone { get; set; }
        public string UserType { get; set; }

        [NotMapped] //this data anontation means that this property will not be apart of the movie table
        public IFormFile Image { get; set; } //IFromFile is not a data type which can be recognised by the database
        public string ImageUrl { get; set; } // will be recognised by DB because it is of type string


        //forming 1 to many relationship with Product (1 user can have many products)
        public ICollection<Product> products { get; set; }
    }
}
