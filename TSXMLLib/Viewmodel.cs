using System.Text;
using System.Text.Json;
using TSXMLLib.WFControls;
using XSD = Dev.Thesmug.Tsxml.Xsd;

namespace TSXMLLib
{
    public class Viewmodel
    {
        public record class Binding(XSD.Control XSDControl, Control WFControl);
        public record struct Serialization(string Ref, dynamic Value);

        private readonly Dictionary<string, Binding> bindings = [];
        private readonly Dictionary<string, XSD.FormDropdownlist> dropdownLists = [];

        public void Bind(XSD.Control xsdControl, Control wfControl) => bindings.Add(xsdControl.Ref, new(xsdControl, wfControl));
        public void Unbind(string Ref) => bindings.Remove(Ref);
        public void ClearBindings() => bindings.Clear();

        public Binding GetBinding(string Ref) => bindings[Ref];
        public Control GetBindingWFControl(string Ref) => bindings[Ref].WFControl;
        public XSD.Control GetBindingXSDControl(string Ref) => bindings[Ref].XSDControl;

        public void AddDropdownList(XSD.FormDropdownlist dropdownList) => dropdownLists.Add(dropdownList.Ref, dropdownList);
        public void RemoveDropdownList(string Ref) => dropdownLists.Remove(Ref);
        public void ClearDropdownLists() => dropdownLists.Clear();

        public XSD.FormDropdownlist GetDropdownlist(string Ref) => dropdownLists[Ref];

        public string Serialize()
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
                    builder.AppendLine(JsonSerializer.Serialize(new Serialization(item.Key, value)));
                }
            }

            return builder.ToString();
        }
    }
}
