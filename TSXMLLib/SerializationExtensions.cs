namespace Dev.Thesmug.Tsxml.Xsd
{
    public partial class Text()
    {
        public record struct Serialization(string Ref, string Value);
    }

    public partial class Checkbox()
    {
        public record struct Serialization(string Ref, bool Checked);
    }

    public partial class RadiobuttonsRadiobutton()
    {
        public record struct Serialization(string Ref, bool Checked);
    }

    public partial class Datetime()
    {
        public record struct Serialization(string Ref, DateTime Value);
    }

    public partial class Dropdown()
    {
        public record struct Serialization(string Ref, string Value);
    }

    public partial class Reference()
    {
        public record struct Serialization(string Ref, string Value);
    }

    public partial class  Color()
    {
        public record struct Serialization(string Ref, System.Drawing.Color Value);
    }
}
