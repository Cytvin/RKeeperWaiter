using RKeeperWaiter.XmlRequests;

namespace RKeeperWaiter
{
    public class GetRefData : IRequest
    {
        private readonly string _refName = null;
        private readonly string _refItemIdent = null;
        private readonly string _propMask = null;
        private readonly string _onlyActive = null;

        public GetRefData(string refName, string refItemIdent, string propMask, string onlyActive) 
        {
            _refName = refName;
            _refItemIdent = refItemIdent;
            _propMask = propMask;
            _onlyActive = onlyActive;
        }

        public void CreateRequest(RequestBuilder builder)
        {
            builder.AddElementToRoot("RK7Command");
            builder.AddAttributeToLast("CMD", "GetRefData");

            if (_refName != null)
            {
                builder.AddAttributeToLast("RefName", _refName);
            }
            
            if (_refItemIdent != null)
            {
                builder.AddAttributeToLast("RefItemIdent", _refItemIdent);
            }

            if (_propMask != null)
            {
                builder.AddAttributeToLast("PropMask", _propMask);
            }

            if (_onlyActive != null)
            {
                builder.AddAttributeToLast("OnlyActive", _onlyActive);
            }
        }
    }
}
