namespace UnityParser
{
    public class ExcelBoolAttribute : ExcelStringAttribute
    {
        public string falseString { get; protected set; }
        public string trueString { get; protected set; }

        public ExcelBoolAttribute(string name, float width, string falseString, string trueString) : base(name, width)
        {
            this.falseString = falseString;
            this.trueString = trueString;
        }

        public override string ToString(object fieldValue)
        {
            bool b = (bool)fieldValue;
            return b ? trueString : falseString;
        }
    }
}