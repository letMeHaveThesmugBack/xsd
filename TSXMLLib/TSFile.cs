using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TSXMLLib
{
    public abstract class TSFile
    {
        public readonly Uri Source;

        public readonly FileInfo LocalFile;

        protected TSFile(Uri source, FileInfo localFile)
        {
            Source = source;
            LocalFile = localFile;
        }
    }
}
