using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrederAggregat
{
    public class ProductItemOredered
    {
        public ProductItemOredered()
        {
            
        }
        public ProductItemOredered(int ProductItemId,string ProductNamed,string ProductUrl)
        {
            ProductItemId = ProductItemId;
            ProductNamed = ProductNamed;
            ProductUrl = ProductUrl;
        }
        public int ProductItemId { get; set; }
        public string ProductNamed { get; set; }
        public string  ProductUrl { get; set; }
    }
}
