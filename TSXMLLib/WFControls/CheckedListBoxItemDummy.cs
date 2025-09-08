namespace TSXMLLib.WFControls
{
    internal class CheckedListBoxItemDummy(CheckedListBox ContainingBox, int ItemIndex) : Control
    {
        internal CheckedListBox ContainingBox { get; } = ContainingBox;
        internal int ItemIndex { get; } = ItemIndex;
    }
}
