using System.Xml;

namespace RKeeperWaiter
{
    public class RequestBuilder
    {
        private XmlDocument _xmlDocument;
        private XmlElement _root;
        private XmlElement _lastElement;
        private XmlElement _internalElement;

        public RequestBuilder()
        {
            _xmlDocument = new XmlDocument();
            AddStartDocument();
        }

        public void AddElementToRoot(string name)
        {
            _lastElement = _xmlDocument.CreateElement(name);
            _root.AppendChild(_lastElement);
        }

        public void AddElementToLast(string name)
        {
            _internalElement = _xmlDocument.CreateElement(name);
            _lastElement.AppendChild(_internalElement);
        }

        public void AddAttributeToLast(string name, string value)
        {
            XmlAttribute newAttribute = _xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;
            _lastElement.Attributes.Append(newAttribute);
        }

        public void AddAttributToInternal(string name, string value)
        {
            XmlAttribute newAttribute = _xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;
            _internalElement.Attributes.Append(newAttribute);
        }

        private void AddStartDocument()
        {
            XmlDeclaration xmlDeclaration = _xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            _xmlDocument.AppendChild(xmlDeclaration);

            _root = _xmlDocument.CreateElement("RK7Query");
            _xmlDocument.AppendChild(_root);
        }

        public XmlDocument GetXml()
        {
            return _xmlDocument;
        }
    }
}
