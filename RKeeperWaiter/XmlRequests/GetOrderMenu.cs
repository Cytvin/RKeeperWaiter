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
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "GetOrderMenu");
            builder.AddAttribute("PropMask", "Dishes.(Ident,Price),Modifiers.(Ident,Price),OrderTypes.(Ident)");
            builder.AddAttribute("checkrests", "false");

            builder.AddElement("Station");
            builder.AddAttribute("id", _stationId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Waiter");
            builder.AddAttribute("id", _userId.ToString());
        }
    }
}
