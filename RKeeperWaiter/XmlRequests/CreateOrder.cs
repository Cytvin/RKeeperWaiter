namespace RKeeperWaiter.XmlRequests
{
    public class CreateOrder
    {
        private int _tabelId;
        private int _waiterId;
        private int _stationId;
        private int _typeId;
        private int _guestCount;

        public CreateOrder(int tabelId, int waiterId, int stationId, int typeId ,int guestCount)
        {
            _tabelId = tabelId;
            _waiterId = waiterId;
            _stationId = stationId;
            _typeId = typeId;
            _guestCount = guestCount;
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "CreateOrder");
            builder.AddAttribute("checkrests", "false");

            builder.AddElement("Order");

            builder.AddElement("Table");
            builder.AddAttribute("id", _tabelId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Creator");
            builder.AddAttribute("id", _waiterId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Waiter");
            builder.AddAttribute("id", _waiterId.ToString());
            builder.SelectPreviousElement();

            builder.AddElement("Station");
            builder.AddAttribute("id", _stationId.ToString());
            builder.SelectPreviousElement();

            if (_guestCount > 0)
            {
                builder.AddElement("Guests");
                builder.AddAttribute("count", _guestCount.ToString());

                for (int i = 0; i < _guestCount; i++)
                {
                    builder.AddElement("Guest");
                    builder.AddAttribute("GuestLabel", (i + 1).ToString());
                    builder.SelectPreviousElement();
                }
                builder.SelectPreviousElement();
            }

            builder.AddElement("OrderType");
            builder.AddAttribute("id", _typeId.ToString());
            builder.SelectPreviousElement();
        }
    }
}
