using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FoodCatalog.Models
{
    public class Categorys
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<Products> Product { get; set; }
    }

    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public string SpecialNote { get; set; }
        public int CategorysId { get; set; }
        public Categorys Categorys { get; set; }
    }



    public class SearchM
    {
        public IEnumerable<Products> Product { get; set; }
        public SelectList Categor { get; set; }
    }

    public class SelectForEditting
    {
        public Products Product { get; set; }
        public SelectList Categor { get; set; }
    }
}
