using System;

namespace AetherFlow.Xrm.Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class LabelAttribute : Attribute
    {
        public int LanguageCode { get; }
        public string Value { get; }

        public LabelAttribute(int languageCode, string value)
        {
            LanguageCode = languageCode;
            Value = value;
        }
    }
}
