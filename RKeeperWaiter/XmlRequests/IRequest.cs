using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    internal interface IRequest
    {
        void CreateRequest(RequestBuilder builder);
    }
}
