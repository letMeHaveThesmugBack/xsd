using System.Collections.ObjectModel;

namespace TSXMLEdit
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // TODO: this should open the json files which references the xml from my website, and also have an option to make a new json from an xml either locally or from a link
            // TODO: allow drag and drop of link onto the link option
            // TODO: drag and drop change cursor to not allowed if the filetype is invalid for what we are dropping onto
            ApplicationConfiguration.Initialize();
            Application.Run(new Viewer());
        }
    }
}