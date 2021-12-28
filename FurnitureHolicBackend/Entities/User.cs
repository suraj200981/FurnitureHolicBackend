using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //forming 1 to many relationship with Product (1 user can have many products)
        public ICollection<Product> products { get; set; }
    }
}
