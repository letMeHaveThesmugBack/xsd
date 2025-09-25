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
        private TSXMLFile(Uri source, FileInfo localFile) : base(source, localFile) { }

        public XSD.Form? Form { get; private set; }

        static string ITSFileFactory<TSXMLFile>.Extension => ".tsxml";

        public static async Task<TSXMLFile?> CreateCoreAsync(Uri source, FileInfo localFile, CancellationToken cancellationToken)
        {
            try
            {
                XmlSerializer serializer = new(typeof(XSD.Form));
                XmlReaderSettings settings = new()
                {
                    Async = true
                };

                using StreamReader reader = new(localFile.FullName);
                using XmlReader xreader = XmlReader.Create(reader, settings);

                await xreader.MoveToContentAsync();
                XSD.Form? form = serializer.Deserialize(xreader) as XSD.Form;

                TSXMLFile xfile = new(source, localFile)
                {
                    Form = form
                };

                return xfile;
            }

            catch
            {
                return null; // TODO: handle errors
            }
        }
    }
}
