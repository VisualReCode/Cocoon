using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerCocoon.Data
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required, StringLength(100), Display(Name = "Name")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Product Description")]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = null!;
    }
}