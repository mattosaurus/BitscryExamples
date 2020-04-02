using System;
using System.Collections.Generic;

namespace OData.Models.AdventureWorks
{
    public partial class ProductModel
    {
        public ProductModel()
        {
            Product = new HashSet<Product>();
            ProductModelProductDescriptions = new HashSet<ProductModelProductDescription>();
        }

        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ProductModelProductDescription> ProductModelProductDescriptions { get; set; }
    }
}
