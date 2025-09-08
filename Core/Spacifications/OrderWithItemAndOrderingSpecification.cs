using Core.Entities.OrederAggregat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Spacifications
{
    public class OrderWithItemAndOrderingSpecification : BaseSpacification<Order>
    {
        public OrderWithItemAndOrderingSpecification(string Email):base(o=>o.BuyerEmail == Email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);

            ApplyOrderByDescending(o => o.OrderDate);
        }
        public OrderWithItemAndOrderingSpecification(int id,string Email): base(o => o.Id == id && o.BuyerEmail == Email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);


        }

    }
}
