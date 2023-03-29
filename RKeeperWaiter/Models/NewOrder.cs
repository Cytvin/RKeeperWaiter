using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class NewOrder
    {
        private int _guestCount;
        private int _waiterId;
        private int _stationId;
        private Table _table;
        private Hall _hall;
        private int _orderType;

        public int GuestCount { get { return _guestCount; } }
        public Table Table { get { return _table; } }
        public Hall Hall { get { return _hall; } }

        public void SetWaiterId(int waiterId)
        {
            _waiterId = waiterId;
        }

        public void SetStationId(int stationId)
        {
            _stationId = stationId;
        }

        public void SetTableAndTable(Hall hall, Table table) 
        {
            _hall = hall;
            _table = table;
        }

        public void SetGuestCount(int guestCount)
        {
            if (guestCount < 0)
            {
                _guestCount = 0;
                return;
            }

            _guestCount = guestCount;
        }

        public void SetOrderType(int orderType)
        {
            _orderType = orderType;
        }
    }
}
