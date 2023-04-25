using RKeeperWaiter.XmlRequests;

namespace RKeeperWaiter
{
    public class GetRefData : IRequest
    {
        private readonly string _refName = null;
        private readonly string _withChildItems = null;
        private readonly string _refItemIdent = null;
        private readonly string _propMask = null;
        private readonly string _onlyActive = null;

        public GetRefData(string refName, string withChildItems, string refItemIdent, string propMask, string onlyActive) 
        {
            _refName = refName;
            _withChildItems = withChildItems;
            _refItemIdent = refItemIdent;
            _propMask = propMask;
            _onlyActive = onlyActive;
        }

        public void CreateRequest(RequestBuilder builder)
        {

            builder.AddElement("RK7Command");
            builder.AddAttribute("CMD", "GetRefData");

            if (_refName != null)
            {
                builder.AddAttribute("RefName", _refName);
            }
            
            if (_withChildItems != null)
            {
                builder.AddAttribute("WithChildItems", _withChildItems);
            }

            if (_refItemIdent != null)
            {
                builder.AddAttribute("RefItemIdent", _refItemIdent);
            }

            if (_propMask != null)
            {
                builder.AddAttribute("PropMask", _propMask);
            }

            if (_onlyActive != null)
            {
                builder.AddAttribute("OnlyActive", _onlyActive);
            }
        }
    }
}
