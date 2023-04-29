using System;
using System.Collections.Generic;
using System.IO;
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
        private Dictionary<int, List<Dish>> _courses;

        public SaveOrder(Order orderToSave, License license, int stationId, int author)
        {
            _order = orderToSave;
            _license = license;
            _stationId = stationId;
            _authorId = author;
            _courses = CreateCoursesList();
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

            foreach (KeyValuePair<int, List<Dish>> courseDishes in _courses)
            {
                builder.AddElement("Session");

                builder.AddElement("Author");
                builder.AddAttribute("id", _authorId.ToString());
                builder.SelectPreviousElement();

                builder.AddElement("Creator");
                builder.AddAttribute("id", _authorId.ToString());
                builder.SelectPreviousElement();

                if (courseDishes.Key != 0)
                {
                    builder.AddElement("Course");
                    builder.AddAttribute("id", courseDishes.Key.ToString());
                    builder.SelectPreviousElement();
                }

                foreach (Dish dish in courseDishes.Value)
                {
                    AddDish(builder, dish);
                }

                builder.SelectPreviousElement();
            }
        }

        private void AddDish(RequestBuilder builder, Dish dish)
        {
            builder.AddElement("Dish");
            builder.AddAttribute("id", dish.Id.ToString());
            builder.AddAttribute("quantity", "1000");
            builder.AddAttribute("seat", dish.Seat);

            foreach (ModifiersGroup modifiersGroup in dish.ModifiersSheme.ModifiersGroups)
            {
                foreach (Modifier modifier in modifiersGroup.Modifiers)
                {
                    if (modifier.Selected)
                    {
                        builder.AddElement("Modi");
                        builder.AddAttribute("id", modifier.Id.ToString());
                        builder.AddAttribute("count", modifier.Count.ToString());
                        builder.AddAttribute("price", modifier.Price.ToString());
                        builder.SelectPreviousElement();
                    }
                }
            }

            builder.SelectPreviousElement();
        }

        private Dictionary<int, List<Dish>> CreateCoursesList()
        {
            Dictionary<int, List<Dish>> coursesDishesDictionary = new Dictionary<int, List<Dish>>();

            AddDishesToCourses(coursesDishesDictionary, _order.CommonDishes, "0");

            foreach(Guest guest in _order.Guests)
            {
                AddDishesToCourses(coursesDishesDictionary, guest.Dishes, guest.Label);
            }

            return coursesDishesDictionary;
        }

        private void AddDishesToCourses(Dictionary<int, List<Dish>> outputDictionary, IEnumerable<Dish> dishes, string seat)
        {
            foreach (Dish dish in dishes)
            {
                dish.Seat = seat;
                if (outputDictionary.ContainsKey(dish.Course.Id) == false)
                {
                    outputDictionary.Add(dish.Course.Id, new List<Dish>());
                }

                outputDictionary[dish.Course.Id].Add(dish);
            }
        }
    }
}
