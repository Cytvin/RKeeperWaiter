namespace RKeeperWaiter.Models
{
    public class GuestType
    {
        private int _id;
        private int _code;
        private string _name;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;

        public GuestType(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }
    }
}
