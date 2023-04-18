using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Course
    {
        private int _id;
        private string _name;

        public static Course Empty => new Course(0, "Не указан");
        public int Id => _id;
        public string Name => _name;

        public Course(int id, string name) 
        {
            _id = id;
            _name = name;
        }
    }
}
