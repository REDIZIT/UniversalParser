using System;

namespace UnityParser
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelStringAttribute : Attribute
    {
		public string name { get; protected set; }
        public ExcelStringAttribute(string name)
        {
			this.name = name;
        }

        public virtual string ToString(object fieldValue)
        {
            return fieldValue.ToString();
        }
    }
}