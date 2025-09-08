using Core.Entities.OrederAggregat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Spacifications
{
    public class OrderByPaymentIntentIdSpacefication:BaseSpacification<Order>
    {
        public OrderByPaymentIntentIdSpacefication(string PaymentIntentId):base(o=>o.PaymentIntentId== PaymentIntentId)
        {

            
        }
    }
}
