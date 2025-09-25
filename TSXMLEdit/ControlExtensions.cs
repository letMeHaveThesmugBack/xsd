using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSXMLLib.WFControls;

namespace TSXMLEdit
{
    internal interface IReportsChanges // TODO: this is a weird split between the interface and class. most of this should be in the class i think
    {
        internal void SubscribeChangeHandler(EventHandler handler) { }
        internal void UnsubscribeChangeHandler(EventHandler handler) { }
        internal virtual void ToggleEditing(bool enable) { }

        internal static IReportsChanges? Create<TOriginator>(TOriginator originator) where TOriginator : Control => originator switch
        {
            TextBox textBox => new TextBoxEx(textBox),
            CheckBox checkBox => new CheckBoxEx(checkBox),
            CheckedListBoxItemDummy checkDummy => new CheckedListBoxItemDummyEx(checkDummy),
            RadioButton radioButton => new RadioButtonEx(radioButton),
            DateTimePicker dateTimePicker => new DateTimePickerEx(dateTimePicker),
            ComboBox comboBox => new ComboBoxEx(comboBox),
            Reference reference => new ReferenceEx(reference),
            ColorPicker colorPicker => new ColorPickerEx(colorPicker),
            _ => null,
        };

        internal virtual void Reset(object? sender, EventArgs e) { }

        internal abstract Control GetControl();
    }

    internal abstract class ControlEx<T>(T originator) : IReportsChanges where T : Control
    {
        internal readonly T Originator = originator;

        Control IReportsChanges.GetControl() => Originator;
    }

    internal class TextBoxEx(TextBox originator) : ControlEx<TextBox>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.TextChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.TextChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.ReadOnly = !enable;

        void IReportsChanges.Reset(object? sender, EventArgs e) => Originator.Clear();
    }

    internal class CheckBoxEx(CheckBox originator) : ControlEx<CheckBox>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.CheckedChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.CheckedChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.Enabled = enable;

        void IReportsChanges.Reset(object? sender, EventArgs e) => Originator.Checked = false;
    }

    internal class CheckedListBoxItemDummyEx(CheckedListBoxItemDummy originator) : ControlEx<CheckedListBoxItemDummy>(originator), IReportsChanges
    {
        private readonly Dictionary<EventHandler, ItemCheckEventHandler> eventHandlerAdapters = [];

        void IReportsChanges.SubscribeChangeHandler(EventHandler handler)
        {
            if (!eventHandlerAdapters.ContainsKey(handler))
            {
                void Adapter(object? s, ItemCheckEventArgs e) => handler(s, e);
                eventHandlerAdapters[handler] = Adapter;
                Originator.ContainingBox.ItemCheck += Adapter;
            }
        }

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler)
        {
            if (eventHandlerAdapters.TryGetValue(handler, out var adapter))
            {
                Originator.ContainingBox.ItemCheck -= adapter;
                eventHandlerAdapters.Remove(handler);
            }
        }

        void IReportsChanges.ToggleEditing(bool enable) => Originator.ContainingBox.SelectionMode = enable ? SelectionMode.One : SelectionMode.None;
    }

    internal class RadioButtonEx(RadioButton originator) : ControlEx<RadioButton>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.CheckedChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.CheckedChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.Enabled = enable;

        void IReportsChanges.Reset(object? sender, EventArgs e) => Originator.Checked = false;
    }

    internal class DateTimePickerEx(DateTimePicker originator) : ControlEx<DateTimePicker>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.ValueChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.ValueChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.Enabled = enable;

        void IReportsChanges.Reset(object? sender, EventArgs e) => Originator.Value = DateTime.MinValue;
    }

    internal class ComboBoxEx(ComboBox originator) : ControlEx<ComboBox>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.SelectedValueChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.SelectedValueChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.Enabled = enable;

        void IReportsChanges.Reset(object? sender, EventArgs e) => Originator.SelectedIndex = -1;
    }

    internal class ReferenceEx(Reference originator) : ControlEx<Reference>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) { } // TODO: Implement

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) { }

        void IReportsChanges.ToggleEditing(bool enable) { } // TODO: general thing, this is a terrible architecture. these enable/disable and change reporting things should all be handled by the binding, not in here
    }

    internal class ColorPickerEx(ColorPicker originator) : ControlEx<ColorPicker>(originator), IReportsChanges
    {
        void IReportsChanges.SubscribeChangeHandler(EventHandler handler) => Originator.BackColorChanged += handler;

        void IReportsChanges.UnsubscribeChangeHandler(EventHandler handler) => Originator.BackColorChanged -= handler;

        void IReportsChanges.ToggleEditing(bool enable) => Originator.Enabled = enable;
    }
}
