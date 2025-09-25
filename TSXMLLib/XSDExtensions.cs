using System.ComponentModel.Design;
using TSXMLLib;
using TSXMLLib.WFControls;
using WF = System.Windows.Forms;

namespace Dev.Thesmug.Tsxml.Xsd
{
    public partial class Control
    {
        protected static readonly Font entryFont = new("Letter Gothic Std", 8.25f, FontStyle.Bold);

        public virtual WF.Control? Instantiate(TableLayoutPanel panel, Viewmodel viewmodel, bool enabled = false)
        {
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            WF.Control? control = InstantiateCore(viewmodel, enabled);

            WF.Label prompt = CreateStandardLabel();
            prompt.Text = Prompt;

            panel.Controls.Add(prompt, 0, panel.RowCount - 1);

            if (control is not null)
            {
                control.Dock = DockStyle.Left;
                panel.Controls.Add(control, 1, panel.RowCount - 1);
            }

            return control;
        }

        internal static TableLayoutPanel CreateStandardTableLayoutPanel()
        {
            TableLayoutPanel panel = new()
            {
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill
            };

            panel.ColumnStyles.Add(new(SizeType.AutoSize));
            panel.ColumnStyles.Add(new(SizeType.AutoSize));

            return panel;
        }

        protected static WF.Label CreateStandardLabel() => new()
        {
            TextAlign = ContentAlignment.MiddleLeft,
            AutoSize = true,
            Dock = DockStyle.None,
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
        };

        protected abstract WF.Control? InstantiateCore(Viewmodel viewmodel, bool enabled = false);
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
                         .OrderByDescending(x => x.Priority)
                         .ThenBy(x => x.Ref); // TODO: make reusable (same code repeated elsewhere)
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
                         .OrderByDescending(x => x.Priority)
                         .ThenBy(x => x.Ref);

        public override GroupBox Instantiate(TableLayoutPanel panel, Viewmodel viewmodel, bool enabled = false)
        {
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            GroupBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            panel.Controls.Add(box, 0, panel.RowCount - 1);
            panel.SetColumnSpan(box, 2);

            viewmodel.Bind(this, box);

            TableLayoutPanel subpanel = CreateStandardTableLayoutPanel();

            box.Controls.Add(subpanel);

            foreach (Control control in Controls)
            {
                control.Instantiate(subpanel, viewmodel);
            }

            return box;
        }

        protected override WF.Control? InstantiateCore(Viewmodel viewmodel, bool enabled = false) => null;
    }

    public partial class Label
    {
        protected override WF.Control? InstantiateCore(Viewmodel viewmodel, bool enabled = false) => null;
    }

    public partial class Link
    {
        protected override LinkLabel InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            LinkLabel lbl = new()
            {
                Text = "TODO",
                Name = Ref,
                Tag = this
            };

            viewmodel.Bind(this, lbl);

            return lbl;
        }
    }

    public partial class Text
    {
        const int maxWidth = 320;
        const int maxHeight = 160;
        const int widthPadding = 6;
        const int heightPadding = 3;

        protected override TextBox InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            TextBox box = new()
            {
                Name = Ref,
                Tag = this,
                MaxLength = Length,
                Font = entryFont,
                ReadOnly = !enabled
            };

            using (var g = WF.Form.ActiveForm?.CreateGraphics() ?? new WF.Form().CreateGraphics())
            {
                string sample = new('W', Length > 0 ? Length : 1);
                int singleLineWidth = TextRenderer.MeasureText(g, sample, box.Font).Width;

                if (singleLineWidth + widthPadding > maxWidth)
                {
                    box.Multiline = true;
                    box.WordWrap = true;

                    int multiLineHeight = box.Font.Height * (int)Math.Ceiling((double)singleLineWidth / maxWidth) + heightPadding;

                    if (multiLineHeight > maxHeight)
                    {
                        box.Height = maxHeight;
                        box.ScrollBars = ScrollBars.Vertical;
                    }

                    box.Width = maxWidth;
                }

                else
                {
                    box.Multiline = false;
                    box.WordWrap = false;
                    box.Width = singleLineWidth + widthPadding;
                }
            }

            viewmodel.Bind(this, box);

            return box;
        }
    }

    public partial class Checkbox
    {
        public override string ToString() => Prompt;

        protected override CheckBox InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            CheckBox box = new()
            {
                Name = Ref,
                Tag = this,
                Enabled = enabled
            };

            viewmodel.Bind(this, box);

            return box;
        }
    }

    public partial class Checkedlistbox
    {
        protected override CheckedListBox InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            const int widthPadding = 48;
            const int maxWidth = 320;

            CheckedListBox box = new()
            {
                Name = Ref,
                Tag = this,
                Font = entryFont
            };

            viewmodel.Bind(this, box);

            foreach (Checkbox subcheck in Checkbox)
            {
                int index = box.Items.Add(subcheck);
                box.SelectionMode = enabled ? SelectionMode.One : SelectionMode.None;

                viewmodel.Bind(subcheck, new CheckedListBoxItemDummy(box, index));
            }

            using (Graphics graphics = WF.Form.ActiveForm?.CreateGraphics() ?? new WF.Form().CreateGraphics()) // TODO: DRY (textbox uses the same code)
            {
                int longestStringLength = (from item
                                           in Checkbox
                                           orderby item.Prompt.Length descending
                                           select item.Prompt.Length).FirstOrDefault();

                string sample = new('W', longestStringLength > 0 ? longestStringLength : 1);
                int singleLineWidth = TextRenderer.MeasureText(graphics, sample, box.Font).Width;

                if (singleLineWidth + widthPadding > maxWidth)
                {
                    box.Width = maxWidth;
                    box.HorizontalScrollbar = true;
                }
                else
                {
                    box.Width = singleLineWidth + widthPadding;
                    box.HorizontalScrollbar = false;
                }
            }

            return box;
        }
    }

    public partial class Radiobuttons
    {
        public override WF.Control? Instantiate(TableLayoutPanel panel, Viewmodel viewmodel, bool enabled = false)
        {
            GroupBox box = new()
            {
                Text = Prompt,
                Name = Ref,
                Tag = this,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
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
                    Tag = button,
                    Enabled = enabled
                };
                subpanel.Controls.Add(rb);

                viewmodel.Bind(button, rb);
            }

            return box;
        }

        protected override WF.Control? InstantiateCore(Viewmodel viewmodel, bool enabled = false) => null;
    }

    public partial class RadiobuttonsRadiobutton
    {
        protected override WF.Control? InstantiateCore(Viewmodel viewmodel, bool enabled = false) => null;
    }

    public partial class Datetime
    {
        protected override DateTimePicker InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            DateTimePicker picker = new()
            {
                Name = Ref,
                Tag = this,
                Font = entryFont,
                Enabled = enabled
            };

            viewmodel.Bind(this, picker);

            return picker;
        }
    }

    public partial class Dropdown
    {
        protected override TableLayoutPanel InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            TableLayoutPanel subpanel = CreateStandardTableLayoutPanel();

            ComboBox box = new()
            {
                Name = Ref,
                Tag = this,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new(0, 3, 3, 3),
                Font = entryFont,
                Enabled = enabled
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

            WF.Label label = CreateStandardLabel();
            subpanel.Controls.Add(label);

            box.SelectedIndexChanged += (s, e) =>
            {
                if (box.SelectedItem is Dropdownvalue value)
                {
                    label.Text = value.Description;
                }

                else
                {
                    label.Text = string.Empty;
                }
            };

            return subpanel;
        }
    }

    public partial class Dropdownvalue
    {
        public override string ToString() => Value;
    }

    public partial class Reference
    {
        protected override LinkLabel InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            LinkLabel label = new()
            {
                Name = Ref,
                Tag = this,
                AllowDrop = true
                // TODO: implement handling for enabled
            };

            viewmodel.Bind(this, label);

            // TODO: handle dropping

            return label;
        }
    }

    public partial class Color
    {
        protected override ColorPicker InstantiateCore(Viewmodel viewmodel, bool enabled = false)
        {
            ColorPicker picker = new()
            {
                Name = Ref,
                Tag = this,
                BackColor = System.Drawing.Color.White,
                Enabled = enabled
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

            viewmodel.Bind(this, picker);

            return picker;
        }
    }
}
