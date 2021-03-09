using System.Collections.Generic;

namespace BlazorCocoon.Shared
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = null!;
    }
}