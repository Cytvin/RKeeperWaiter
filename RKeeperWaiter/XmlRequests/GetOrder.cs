using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    public class GetOrder : IRequest
    {
        private string _orderGuid;

        public GetOrder(Guid orderGuid)
        {
            _orderGuid = $"{{{orderGuid.ToString().ToUpper()}}}";
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElementToRoot("RK7Command");
            builder.AddAttributeToLast("CMD", "GetOrder");
            builder.AddElementToLast("Order");
            builder.AddAttributToInternal("guid", _orderGuid);
        }
    }
}
