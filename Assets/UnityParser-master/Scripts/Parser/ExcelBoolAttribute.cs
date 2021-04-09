namespace UnityParser
{
    public class ExcelBoolAttribute : ExcelStringAttribute
    {
        public string falseString { get; protected set; }
        public string trueString { get; protected set; }

        public ExcelBoolAttribute(string name, string falseString, string trueString) : base(name)
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