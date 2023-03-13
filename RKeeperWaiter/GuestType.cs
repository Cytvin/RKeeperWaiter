namespace RKeeperWaiter
{
    internal class GuestType
    {
        private int _id;
        private int _code;
        private string _name;

        public int Id { get { return _id; } }
        public int Code { get { return _code; } }
        public string Name { get { return _name; } }

        public GuestType(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }
    }
}
