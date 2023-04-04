using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    public class GetWaiterList : IRequest
    {
        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "GetWaiterList");
            builder.AddAttribute("checkrests", "false");
            builder.AddAttribute("RegisteredOnly", "1");
        }
    }
}
