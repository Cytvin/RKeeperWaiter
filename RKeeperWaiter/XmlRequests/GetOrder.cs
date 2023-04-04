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
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "GetOrder");

            builder.AddElement("Order");
            builder.AddAttribute("guid", _orderGuid);
        }
    }
}
