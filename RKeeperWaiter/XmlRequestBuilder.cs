using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RKeeperWaiter
{
    public class XmlRequestBuilder
    {
        private XmlWriter _xmlWriter;
        private StringBuilder _xmlText;

        public XmlRequestBuilder()
        {
            _xmlText = new StringBuilder();
            _xmlWriter = XmlWriter.Create(_xmlText);
            AddStartDocument();
        }

        public void AddElement(string name)
        {
            _xmlWriter.WriteStartElement(name);
            _xmlWriter.Flush();
        }

        public void AddAttribute(string name, string value)
        {
            _xmlWriter.WriteAttributeString(name, value);
            _xmlWriter.Flush();
        }

        public void AddEndElement()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.Flush();
        }

        private void AddStartDocument()
        {
            _xmlWriter.WriteStartDocument();
            _xmlWriter.Flush();
        }

        public string GetRequestText()
        {
            return _xmlText.ToString();
        }
    }
}
