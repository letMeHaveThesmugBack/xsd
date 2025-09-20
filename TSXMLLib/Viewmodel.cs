using System.CodeDom;
using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using TSXMLLib.WFControls;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    public class Viewmodel
    {
        public record class Binding(XSD.Control XSDControl, Control WFControl);
        public record struct Serialization(string Ref, dynamic Value);

        public Form? WForm { get; private set; }

        private readonly Dictionary<string, Binding> bindings = [];
        private readonly Dictionary<string, XSD.FormDropdownlist> dropdownLists = [];

        public ReadOnlyDictionary<string, Binding> Bindings { get => bindings.AsReadOnly(); }

        public void SetBindingValues(ReadOnlyCollection<Serialization> serializations)
        {
            foreach (Serialization serialization in serializations)
            {
                switch (bindings[serialization.Ref].WFControl, serialization.Value)
                {
                    case (TextBox textBox, string textBoxValue):
                        textBox.Text = textBoxValue;
                        break;
                    case (CheckBox checkBox, bool checkBoxValue):
                        checkBox.Checked = checkBoxValue;
                        break;
                    case (CheckedListBoxItemDummy checkDummy, bool checkDummyValue):
                        checkDummy.ContainingBox.SetItemChecked(checkDummy.ItemIndex, checkDummyValue);
                        break;
                    case (RadioButton radioButton, bool radioButtonValue):
                        radioButton.Checked = radioButtonValue;
                        break;
                    case (DateTimePicker dateTimePicker, DateTime dateTimeValue):
                        dateTimePicker.Value = dateTimeValue;
                        break;
                    case (ComboBox comboBox, string comboBoxValue):
                        comboBox.Text = comboBoxValue;
                        break;
                    case (Reference reference, string referenceValue):
                        reference.Text = referenceValue; // TODO: implement
                        break;
                    case (ColorPicker colorPicker, Color colorValue):
                        colorPicker.BackColor = colorValue;
                        break;
                    default:
                        // TODO: error log invalid binding serializations, possibly also send a warning message that there were invalid bindings (but don't crash)
                        break;
                }
            }
        }

        internal void Bind(XSD.Control xsdControl, Control wfControl) => bindings.Add(xsdControl.Ref, new(xsdControl, wfControl));
        internal XSD.FormDropdownlist GetDropdownlist(string Ref) => dropdownLists[Ref];

        public string Serialize() // TODO: should be async
        {
            StringBuilder builder = new();

            foreach (KeyValuePair<string, Binding> item in bindings)
            {
                dynamic? value = item.Value.WFControl switch
                {
                    TextBox textBox => textBox.Text,
                    CheckBox checkBox => checkBox.Checked,
                    CheckedListBoxItemDummy checkDummy => checkDummy.ContainingBox.GetItemChecked(checkDummy.ItemIndex),
                    RadioButton radioButton => radioButton.Checked,
                    DateTimePicker dateTimePicker => dateTimePicker.Value,
                    ComboBox comboBox => comboBox.Text,
                    Reference reference => "TODO!", // TODO: implement
                    ColorPicker colorPicker => colorPicker.BackColor,
                    _ => null
                };

                if (value is not null)
                {
                    builder.AppendLine(JsonSerializer.Serialize(new Serialization(item.Key, value), TSNDJFile.JsonOptions));
                }
            }

            return builder.ToString();
        }

        public ContainerControl Render(XSD.Form form)
        {
            ContainerControl container = new();
            TabControl tabControl = new()
            {
                Dock = DockStyle.Fill
            };

            container.Controls.Add(tabControl);

            foreach (XSD.FormDropdownlist dropdownList in form.Dropdownlist)
                dropdownLists.Add(dropdownList.Ref, dropdownList);

            foreach (XSD.FormTab tab in form.Tab)
            {
                TabPage page = new()
                {
                    Text = tab.Name
                };

                Panel scrollPanel = new()
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                TableLayoutPanel panel = XSD.Control.CreateStandardTableLayoutPanel();
                panel.Dock = DockStyle.None;
                panel.AutoSize = true;
                panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                scrollPanel.Controls.Add(panel);
                page.Controls.Add(scrollPanel);
                tabControl.TabPages.Add(page);

                foreach (XSD.Control control in tab.Controls)
                {
                    control.Instantiate(panel, this);
                }
            }

            return container;
        }
    }
}