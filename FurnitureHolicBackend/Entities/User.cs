using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureHolicBackend.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        //forming 1 to many relationship with Product (1 user can have many products)
        public ICollection<Product> products { get; set; }
    }
}
