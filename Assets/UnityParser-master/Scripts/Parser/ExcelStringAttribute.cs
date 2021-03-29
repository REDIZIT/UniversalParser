using System;

namespace UnityParser
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelStringAttribute : Attribute
    {
		public string name { get; protected set; }
        public float width { get; protected set; } = 100;

        public ExcelStringAttribute(string name, float width)
        {
			this.name = name;
            this.width = width;
        }

        public virtual string ToString(object fieldValue)
        {
            return fieldValue.ToString();
        }
    }
}