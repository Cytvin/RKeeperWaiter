using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RKeeperWaiter.Models;

namespace RKeeperWaiter.XmlRequests
{
    public class SaveOrder : IRequest
    {
        private Order _order;
        private License _license;
        private int _stationId;
        private int _authorId;

        public SaveOrder(Order orderToSave, License license, int stationId, int author)
        {
            _order = orderToSave;
            _license = license;
            _stationId = stationId;
            _authorId = author;
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "SaveOrder");
            builder.AddAttribute("lockguid", "");

            builder.AddElement(_license.GetXMLLicense());
            builder.SelectPreviousElement();

            builder.AddElement("Order");
            builder.AddAttribute("guid", $"{{{_order.Guid.ToString().ToUpper()}}}");
            builder.SelectPreviousElement();

            builder.AddElement("LockStation");
            builder.AddAttribute("id", _stationId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Author");
            builder.AddAttribute("id", _authorId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Creator");
            builder.AddAttribute("id", _authorId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Session");

            builder.AddElement("Author");
            builder.AddAttribute("id", _authorId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Creator");
            builder.AddAttribute("id", _authorId.ToString());
            builder.SelectPreviousElement();

            foreach(Dish dish in _order.CommonDishes)
            {
                AddDish(builder, dish.Id, 1000, "0");
            }

            foreach(Guest guest in _order.Guests)
            {
                foreach(Dish dish in guest.Dishes)
                {
                    AddDish(builder, dish.Id, 1000, guest.Label);
                }
            }

            builder.SelectPreviousElement();
        }

        private void AddDish(RequestBuilder builder, int id, int quantity, string seat)
        {
            builder.AddElement("Dish");
            builder.AddAttribute("id", id.ToString());
            builder.AddAttribute("quantity", quantity.ToString());
            builder.AddAttribute("seat", seat);
            builder.SelectPreviousElement();
        }
    }
}
