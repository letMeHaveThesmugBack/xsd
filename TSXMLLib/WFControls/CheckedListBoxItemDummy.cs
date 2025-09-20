namespace TSXMLLib.WFControls
{
    public class CheckedListBoxItemDummy(CheckedListBox ContainingBox, int ItemIndex) : Control
    {
        public CheckedListBox ContainingBox { get; } = ContainingBox;
        public int ItemIndex { get; } = ItemIndex;
    }
}
