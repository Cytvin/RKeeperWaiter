using System.Collections.Generic;
using System.Xml;

namespace RKeeperWaiter
{
    public class RequestBuilder
    {
        private XmlDocument _xmlDocument;

        private Stack<XmlElement> _elements;

        public RequestBuilder()
        {
            _xmlDocument = new XmlDocument();

            _elements = new Stack<XmlElement>();

            AddStartDocument();
        }

        public void AddElement(string name)
        {
            XmlElement xmlElement = _xmlDocument.CreateElement(name);
            _elements.Peek().AppendChild(xmlElement);
            _elements.Push(xmlElement);
        }

        public void AddElement(XmlElement element)
        {
            XmlElement xmlElement = _xmlDocument.ImportNode(element, true) as XmlElement;
            _elements.Peek().AppendChild(xmlElement);
            _elements.Push(xmlElement);
        }

        public void AddAttribute(string name, string value)
        {
            XmlAttribute newAttribute = _xmlDocument.CreateAttribute(name);
            newAttribute.Value = value;

            _elements.Peek().Attributes.Append(newAttribute);
        }

        public void SelectPreviousElement()
        {
            if (_elements.Count == 1)
            {
                return;
            }
            _elements.Pop();
        }

        private void AddStartDocument()
        {
            XmlDeclaration xmlDeclaration = _xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            _xmlDocument.AppendChild(xmlDeclaration);

            XmlElement rk7Query = _xmlDocument.CreateElement("RK7Query");
            _xmlDocument.AppendChild(rk7Query);
            _elements.Push(rk7Query);
        }

        public XmlDocument GetXml()
        {
            return _xmlDocument;
        }
    }
}
