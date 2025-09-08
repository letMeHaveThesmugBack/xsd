using TSXMLLib;
using TSXMLLib.WFControls;
using WF = System.Windows.Forms;

namespace Dev.Thesmug.Tsxml.Xsd
{
    public partial class Control
    {
        public abstract WF.Control Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel);
    }

    public partial class FormTab
    {
        internal IEnumerable<Control> Controls => 
            GroupProperty.Cast<Control>()
                         .Concat(Label)
                         .Concat(Link)
                         .Concat(Text)
                         .Concat(Checkbox)
                         .Concat(Checkedlistbox)
                         .Concat(Radiobuttons)
                         .Concat(Datetime)
                         .Concat(Dropdown)
                         .Concat(Reference)
                         .Concat(Color)
                         .OrderBy(x => x.Ref);
    }

    public partial class Group
    {
        internal IEnumerable<Control> Controls =>
            GroupProperty.Cast<Control>()
                         .Concat(Label)
                         .Concat(Link)
                         .Concat(Text)
                         .Concat(Checkbox)
                         .Concat(Checkedlistbox)
                         .Concat(Radiobuttons)
                         .Concat(Datetime)
                         .Concat(Dropdown)
                         .Concat(Reference)
                         .Concat(Color)
                         .OrderBy(x => x.Ref);

        public override GroupBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            GroupBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(box);

            viewmodel.Bind(this, box);

            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.TopDown
            };
            box.Controls.Add(subpanel);

            foreach (Control control in Controls)
            {
                control.Instantiate(subpanel, viewmodel);
            }

            return box;
        }
    }

    public partial class Label
    {
        public override WF.Label Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            WF.Label lbl = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(lbl);

            viewmodel.Bind(this, lbl);

            return lbl;
        }
    }

    public partial class Link
    {
        public override LinkLabel Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            LinkLabel lbl = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(lbl);

            viewmodel.Bind(this, lbl);

            return lbl;
        }
    }

    public partial class Text
    {
        public override TextBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            TextBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                MaxLength = Length
            };
            panel.Controls.Add(box);

            viewmodel.Bind(this, box);

            return box;
        }
    }

    public partial class Checkbox
    {
        public override CheckBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            CheckBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(box);

            viewmodel.Bind(this, box);

            return box;
        }
    }

    public partial class Checkedlistbox
    {
        public override CheckedListBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            CheckedListBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(box);

            viewmodel.Bind(this, box);

            foreach (Checkbox subcheck in Checkbox)
            {
                int index = box.Items.Add(subcheck);

                viewmodel.Bind(subcheck, new CheckedListBoxItemDummy(box, index));
            }

            return box;
        }
    }

    public partial class Radiobuttons
    {
        public override GroupBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            GroupBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(box);

            viewmodel.Bind(this, box);

            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.LeftToRight
            };
            box.Controls.Add(subpanel);

            foreach (RadiobuttonsRadiobutton button in Radiobutton)
            {
                RadioButton rb = new()
                {
                    Text = button.Prompt,
                    Name = button.Ref,
                    Tag = button
                };
                subpanel.Controls.Add(rb);

                viewmodel.Bind(button, rb);
            }

            return box;
        }
    }

    public partial class Datetime
    {
        public override DateTimePicker Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            DateTimePicker picker = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this
            };
            panel.Controls.Add(picker);

            viewmodel.Bind(this, picker);

            return picker;
        }
    }

    public partial class Dropdown
    {
        public override ComboBox Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.LeftToRight
            };
            panel.Controls.Add(subpanel);

            ComboBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            subpanel.Controls.Add(box);

            viewmodel.Bind(this, box);

            foreach (Dropdownvalue value in Value)
            {
                box.Items.Add(value);
            }

            foreach (string dropdownListRef in Listref)
            {
                foreach (Dropdownvalue value in viewmodel.GetDropdownlist(dropdownListRef).Value)
                {
                    box.Items.Add(value);
                }
            }

            WF.Label label = new();
            subpanel.Controls.Add(label);

            box.SelectedIndexChanged += (s, e) =>
            {
                if (box.SelectedItem is Dropdownvalue value)
                {
                    label.Text = value.Description;
                }
            };

            return box;
        }
    }

    public partial class Reference
    {
        public override LinkLabel Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            LinkLabel label = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                AllowDrop = true
            };
            panel.Controls.Add(label);

            viewmodel.Bind(this, label);

            // TODO: handle dropping

            return label;
        }
    }

    public partial class Color
    {
        public override ColorPicker Instantiate(FlowLayoutPanel panel, Viewmodel viewmodel)
        {
            ColorPicker picker = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                BackColor = System.Drawing.Color.White
            };

            picker.Click += (s, e) =>
            {
                using ColorDialog dialog = new();

                dialog.Color = picker.BackColor;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    picker.BackColor = dialog.Color;
                }
            };

            panel.Controls.Add(picker);

            viewmodel.Bind(this, picker);

            return picker;
        }
    }
}
