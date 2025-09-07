namespace Dev.Thesmug.Tsxml.Xsd
{
    internal partial class Group
    {
        internal List<Control> FlattenControls() =>
            [.. GroupProperty.Cast<Control>()
                .Concat(Label)
                .Concat(Link)
                .Concat(Text)
                .Concat(Checkbox)
                .Concat(Checkedlistbox)
                .Concat(Radiobuttons)
                .Concat(Datetime)
                .Concat(Dropdown)
                .Concat(Reference)
                .Concat(Color).OrderBy(x => x.Ref)];
    }
}