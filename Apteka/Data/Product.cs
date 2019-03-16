using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Apteka.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public List<UserProduct> UsersProducts { get; set; }
    }
}
