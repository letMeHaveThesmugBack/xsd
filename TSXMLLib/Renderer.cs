using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    public class Renderer
    {
        public static Form Render(XSD.Form? form, Viewmodel model)
        {
            if (form is null) throw new NotImplementedException(); // TODO: implement something here

            Form winform = new();
            TabControl tabControl = new();

            winform.Controls.Add(tabControl);

            foreach (XSD.FormTab tab in form.Tab)
            {
                TabPage page = new()
                {
                    Text = tab.Name
                };
                tabControl.TabPages.Add(page);

                FlowLayoutPanel panel = new();
                page.Controls.Add(panel);

                foreach (XSD.Control control in tab.Controls)
                {
                    control.Instantiate(panel, model);
                }
            }

            return winform;
        }
    }
}
