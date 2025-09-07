using System.Text;
using System.Text.Json;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    internal static class Serializers
    {
        internal static string Serialize(Form form)
        {
            StringBuilder builder = new();

            foreach (Control control in GetControls(form))
            {
                builder.ValidAppendLine(Serialize(control));
            }

            return builder.ToString();

            static IEnumerable<Control> GetControls(Control control)
            {
                foreach (Control child in control.Controls)
                {
                    yield return child;

                    foreach (Control grandchild in child.Controls)
                    {
                        yield return grandchild;
                    }
                }
            }
        }

        internal static string? Serialize(Control control) => control.Tag switch
        {
            XSD.Group => control is GroupBox groupBox ? SerializeGroup(groupBox) : null,
            XSD.Text => control is TextBox textBox ? SerializeText(textBox) : null,
            XSD.Checkbox => control is CheckBox checkBox ? SerializeCheckBox(checkBox) : null,
            XSD.Checkedlistbox => control is CheckedListBox checkedListBox ? SerializeCheckedListBox(checkedListBox) : null,
            XSD.Radiobuttons => control is GroupBox rbGroupBox ? SerializeRadioButtons(rbGroupBox) : null,
            XSD.Datetime => control is DateTimePicker picker ? SerializeDateTime(picker) : null,
            XSD.Dropdown => control is ComboBox comboBox ? SerializeDropdown(comboBox) : null,
            XSD.Reference => control is LinkLabel link ? SerializeReference(link) : null,
            XSD.Color => control is Button button ? SerializeColor(button) : null,
            _ => null,
        };

        internal static void ValidAppendLine(this StringBuilder builder, string? line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                builder.AppendLine(line);
            }
        }

        internal static string? SerializeGroup(GroupBox box)
        {
            StringBuilder builder = new();

            foreach (Control control in box.Controls)
            {
                builder.ValidAppendLine(Serialize(control));
            }

            return builder.ToString();
        }

        internal static string SerializeText(TextBox box)
        {
            return JsonSerializer.Serialize(new XSD.Text.Serialization(box.Name, box.Text));
        }

        internal static string SerializeCheckBox(CheckBox box)
        {
            return JsonSerializer.Serialize(new XSD.Checkbox.Serialization(box.Name, box.Checked));
        }

        internal static string SerializeCheckedListBox(CheckedListBox box)
        {
            StringBuilder builder = new();

            for (int i = 0; i < box.Items.Count; i++)
            {
                object item = box.Items[i];

                if (item is XSD.Checkbox checkbox)
                {
                    builder.ValidAppendLine(JsonSerializer.Serialize(new XSD.Checkbox.Serialization(checkbox.Ref, box.GetItemChecked(i))));
                }
            }

            return builder.ToString();
        }

        internal static string SerializeRadioButtons(GroupBox box)
        {
            StringBuilder builder = new();

            if (box.Controls[0] is FlowLayoutPanel panel)
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is RadioButton cbutton && cbutton.Tag is XSD.RadiobuttonsRadiobutton xbutton)
                    {
                        builder.ValidAppendLine(JsonSerializer.Serialize(new XSD.RadiobuttonsRadiobutton.Serialization(xbutton.Ref, cbutton.Checked)));
                    }
                }
            }

            return builder.ToString();
        }

        internal static string SerializeDateTime(DateTimePicker picker)
        {
            return JsonSerializer.Serialize(new XSD.Datetime.Serialization(picker.Name, picker.Value));
        }

        internal static string SerializeDropdown(ComboBox box)
        {
            return JsonSerializer.Serialize(new XSD.Dropdown.Serialization(box.Name, box.Text));
        }

        internal static string SerializeReference(LinkLabel label)
        {
            return JsonSerializer.Serialize(new XSD.Reference.Serialization(label.Name, label.Text));
        }

        internal static string SerializeColor(Button button)
        {
            return JsonSerializer.Serialize(new XSD.Color.Serialization(button.Name, button.BackColor));
        }
    }
}