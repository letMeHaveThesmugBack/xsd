using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TSXMLLib
{
    public abstract class TSFile(FileInfo file)
    {
        public readonly FileInfo File = file;
        public Uri? URI { get; internal set; }
    }
}
