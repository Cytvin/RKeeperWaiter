using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.XmlRequests
{
    public class TransferDishes : IRequest
    {
        private int _userId;
        private int _orderSourceId;
        private int _orderDestinationId;
        private string _dishLineGuid;

        public TransferDishes(int userId, int orderSource, int orderDestionation, Guid dishLineGuid) 
        {
            _userId = userId;
            _orderSourceId = orderSource;
            _orderDestinationId = orderDestionation;
            _dishLineGuid = $"{{{dishLineGuid.ToString().ToUpper()}}}";
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "TransferDishes");
            
            builder.AddElement("Dishes");
            
            builder.AddElement("Dish");
            builder.AddAttribute("line_guid", _dishLineGuid);
            builder.SelectPreviousElement();
            
            builder.SelectPreviousElement();

            builder.AddElement("Employee");
            builder.AddAttribute("id", _userId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("OrderDest");
            builder.AddAttribute("orderIdent", _orderDestinationId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("OrderSource");
            builder.AddAttribute("orderIdent", _orderSourceId.ToString());
            builder.SelectPreviousElement();
        }
    }
}
