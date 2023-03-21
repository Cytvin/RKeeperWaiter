using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    public class GetOrderMenu : IRequest
    {
        private readonly int _stationId;
        private readonly int _userId;

        public GetOrderMenu (int stationId, int userId)
        {
            _stationId = stationId;
            _userId = userId;
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElementToRoot("RK7Command");
            builder.AddAttributeToLast("CMD", "GetOrderMenu");
            builder.AddAttributeToLast("PropMask", "Dishes.(Ident,Price),Modifiers.(Ident,Price),OrderTypes.(Ident)");
            builder.AddAttributeToLast("checkrests", "false");

            builder.AddElementToLast("Station");
            builder.AddAttributToInternal("id", _stationId.ToString());

            builder.AddElementToLast("Waiter");
            builder.AddAttributToInternal("id", _userId.ToString());
        }
    }
}
