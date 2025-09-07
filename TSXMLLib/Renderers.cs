using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    internal static class Renderers
    {
        internal static Control Render(this XSD.Control control, FlowLayoutPanel panel) => control switch
        {
            XSD.Group group => group.Render(panel),
            XSD.Label label => label.Render(panel),
            XSD.Link link => link.Render(panel),
            XSD.Text text => text.Render(panel),
            XSD.Checkbox checkbox => checkbox.Render(panel),
            XSD.Checkedlistbox checkedlistbox => checkedlistbox.Render(panel),
            XSD.Radiobuttons radiobuttons => radiobuttons.Render(panel),
            XSD.Datetime datetime => datetime.Render(panel),
            XSD.Dropdown dropdown => dropdown.Render(panel),
            XSD.Reference reference => reference.Render(panel),
            XSD.Color color => color.Render(panel),
            _ => throw new NotImplementedException(),// TODO
        };

        internal static GroupBox Render(this XSD.Group group, FlowLayoutPanel panel)
        {
            GroupBox box = new()
            {
                Text = group.Prompt,
                Name = group.Ref,
                Tag = group
            };
            panel.Controls.Add(box);

            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.TopDown
            };
            box.Controls.Add(subpanel);

            foreach (XSD.Control control in group.FlattenControls())
            {
                control.Render(subpanel);
            }

            return box;
        }

        internal static Label Render(this XSD.Label label, FlowLayoutPanel panel)
        {
            Label lbl = new()
            {
                Text = label.Prompt,
                Name = label.Ref,
                Tag = label
            };
            panel.Controls.Add(lbl);

            return lbl;
        }

        internal static LinkLabel Render(this XSD.Link link, FlowLayoutPanel panel)
        {
            LinkLabel lbl = new()
            {
                Text = link.Prompt,
                Name = link.Ref,
                Tag = link
            };
            panel.Controls.Add(lbl);

            return lbl;
        }

        internal static TextBox Render(this XSD.Text text, FlowLayoutPanel panel)
        {
            TextBox box = new()
            {
                Text = text.Prompt,
                Name = text.Ref,
                Tag = text,
                MaxLength = text.Length
            };
            panel.Controls.Add(box);

            return box;
        }

        internal static CheckBox Render(this XSD.Checkbox check, FlowLayoutPanel panel)
        {
            CheckBox box = new()
            {
                Text = check.Prompt,
                Name = check.Ref,
                Tag = check
            };
            panel.Controls.Add(box);

            return box;
        }

        internal static CheckedListBox Render(this XSD.Checkedlistbox check, FlowLayoutPanel panel)
        {
            CheckedListBox box = new()
            {
                Text = check.Prompt,
                Name = check.Ref,
                Tag = check
            };
            panel.Controls.Add(box);

            foreach (XSD.Checkbox subcheck in check.Checkbox)
            {
                box.Items.Add(subcheck);
            }

            return box;
        }

        internal static GroupBox Render(this XSD.Radiobuttons radiobuttons, FlowLayoutPanel panel)
        {
            GroupBox box = new()
            {
                Text = radiobuttons.Prompt,
                Name = radiobuttons.Ref,
                Tag = radiobuttons
            };
            panel.Controls.Add(box);

            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.LeftToRight
            };
            box.Controls.Add(subpanel);

            foreach (XSD.RadiobuttonsRadiobutton button in radiobuttons.Radiobutton)
            {
                RadioButton rb = new()
                {
                    Text = button.Prompt,
                    Name = button.Ref,
                    Tag = button
                };
                subpanel.Controls.Add(rb);
            }

            return box;
        }

        internal static DateTimePicker Render(this XSD.Datetime datetime, FlowLayoutPanel panel)
        {
            DateTimePicker picker = new()
            {
                Text = datetime.Prompt,
                Name = datetime.Ref,
                Tag = datetime
            };
            panel.Controls.Add(picker);

            return picker;
        }

        internal static ComboBox Render(this XSD.Dropdown dropdown, FlowLayoutPanel panel)
        {
            FlowLayoutPanel subpanel = new()
            {
                FlowDirection = FlowDirection.LeftToRight
            };
            panel.Controls.Add(subpanel);

            ComboBox box = new()
            {
                Text = dropdown.Prompt,
                Name = dropdown.Ref,
                Tag = dropdown,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            subpanel.Controls.Add(box);

            foreach (XSD.Dropdownvalue value in dropdown.Value)
            {
                box.Items.Add(value);
            }

            Label label = new();
            subpanel.Controls.Add(label);

            box.SelectedIndexChanged += (s, e) =>
            {
                if (box.SelectedItem is XSD.Dropdownvalue value)
                {
                    label.Text = value.Description;
                }
            };

            return box;
        }

        internal static LinkLabel Render(this XSD.Reference reference, FlowLayoutPanel panel) // TODO: linklabel is really not the right type for this. maybe a web browser? if we are crazy enough...
        {
            LinkLabel label = new()
            {
                Text = reference.Prompt,
                Name = reference.Ref,
                Tag = reference,
                AllowDrop = true
            };
            panel.Controls.Add(label);

            // TODO: handle dropping

            return label;
        }

        internal static Button Render(this XSD.Color color,  FlowLayoutPanel panel)
        {
            Button button = new()
            {
                Text = color.Prompt,
                Name = color.Ref,
                Tag = color,
                BackColor = Color.White
            };

            button.Click += (s, e) =>
            {
                using ColorDialog dialog = new();

                dialog.Color = button.BackColor;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    button.BackColor = dialog.Color;
                }
            };

            panel.Controls.Add(button);

            return button;
        }
    }
}
