using System;
using System.Collections.Generic;

namespace OData.Models.AdventureWorks
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParentProductCategories = new HashSet<ProductCategory>();
            Product = new HashSet<Product>();
        }

        public int ProductCategoryId { get; set; }
        public int? ParentProductCategoryId { get; set; }
        public string Name { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategory ParentProductCategory { get; set; }
        public virtual ICollection<ProductCategory> InverseParentProductCategories { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
