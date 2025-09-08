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
        public ProductItemOredered(int productItemId,string productName,string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ProductUrl = pictureUrl;
        }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }  // pic Url
    }
}
