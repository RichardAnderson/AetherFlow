using System;
using System.Linq;
using AetherFlow.Xrm.Framework.Core.Attributes;

namespace AetherFlow.Xrm.Framework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToLabel(this Enum value, int languageCode)
        {
            var field = value.GetType().GetField(value.ToString());
            if (!(field.GetCustomAttributes(typeof(Attributes.LabelAttribute), false) is LabelAttribute[] labelAttributes) || labelAttributes.Length == 0)
            {
                return value.ToString();
            }

            var labelAttribute = labelAttributes.FirstOrDefault(a => a.LanguageCode == languageCode);
            return labelAttribute == null ? value.ToString() : labelAttribute.Value;
        }   
    }
}
