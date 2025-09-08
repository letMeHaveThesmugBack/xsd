using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    internal class TSXMLDeserializer
    {
        private readonly XmlSerializer serializer;

        public TSXMLDeserializer()
        {
            serializer = new(typeof(XSD.Form));
        }

        public XSD.Form? Deserialize(string TSXMLFilepath)
        {
            using var reader = new StreamReader(TSXMLFilepath);
            return serializer.Deserialize(reader) as XSD.Form;
        }
    }
}
