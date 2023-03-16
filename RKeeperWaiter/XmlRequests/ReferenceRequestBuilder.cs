using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RKeeperWaiter
{
    public class ReferenceRequestBuilder
    {
        private XmlDocument _xmlDocument;
        private XmlElement _root;
        private XmlElement _lastElement;

        public ReferenceRequestBuilder()
        {
            _xmlDocument = new XmlDocument();
            AddStartDocument();
        }

        public void AddElement(string name)
        {
            XmlElement newElement = _xmlDocument.CreateElement(name);
            _root.AppendChild(newElement);
            _lastElement = newElement;
        }

        public void AddAttribute(string name, string value)
        {
            XmlAttribute newAttribute = _xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;
            _lastElement.Attributes.Append(newAttribute);
        }

        private void AddStartDocument()
        {
            XmlDeclaration xmlDeclaration = _xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            _xmlDocument.AppendChild(xmlDeclaration);

            XmlElement rk7Query = _xmlDocument.CreateElement("RK7Query");
            _xmlDocument.AppendChild(rk7Query);

            _root = rk7Query;
        }

        public XmlDocument GetXml()
        {
            return _xmlDocument;
        }
    }
}
