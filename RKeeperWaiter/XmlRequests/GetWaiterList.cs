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
            builder.AddElementToRoot("RK7Command");
            builder.AddAttributeToLast("CMD", "GetWaiterList");
            builder.AddAttributeToLast("checkrests", "false");
            builder.AddAttributeToLast("RegisteredOnly", "1");
        }
    }
}
