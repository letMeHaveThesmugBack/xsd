using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    public class TSXMLFile : TSFile, ITSFileFactory<TSXMLFile>
    {
        private TSXMLFile(FileInfo file) : base(file) { }

        public XSD.Form? Form { get; private set; }

        static string ITSFileFactory<TSXMLFile>.Extension => ".tsxml";

        public static async Task<TSXMLFile?> CreateFromLocalFileAsyncCore(FileInfo file)
        {
            XmlSerializer serializer = new(typeof(XSD.Form));
            XmlReaderSettings settings = new()
            {
                Async = true
            };

            using StreamReader reader = new(file.FullName);
            using XmlReader xreader = XmlReader.Create(reader, settings);

            await xreader.MoveToContentAsync();
            XSD.Form? form = serializer.Deserialize(xreader) as XSD.Form;

            TSXMLFile xfile = new(file)
            {
                Form = form
            };

            return xfile;
        }
    }
}
