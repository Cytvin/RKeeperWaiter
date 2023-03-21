using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    public class GetOrderList : IRequest
    {
        private readonly int _userId;

        public GetOrderList(int userId)
        {
            _userId = userId;
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElementToRoot("RK7Command");
            builder.AddAttributeToLast("CMD", "GetOrderList");
            builder.AddAttributeToLast("onlyOpened", "true");

            builder.AddElementToLast("Waiter");
            builder.AddAttributToInternal("id", _userId.ToString());
        }
    }
}
