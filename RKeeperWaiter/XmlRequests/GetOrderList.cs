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
            builder.AddAttribute("CMD", "GetOrderList");
            builder.AddAttribute("onlyOpened", "true");

            builder.AddElementToLast("Waiter");
            builder.AddAttribute("id", _userId.ToString());
        }
    }
}
