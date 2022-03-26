using System;
using System.ComponentModel;

namespace Store.Data.Entities
{
    internal static class EnumDescriptionHelper
    {
        public static string GetDescriptionIfExists<TEnum>(this TEnum eventType) where TEnum : struct, Enum
        {
            Type type = eventType.GetType();
            string enumName = Enum.GetName(eventType);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(type.GetField(enumName), typeof(DescriptionAttribute)) 
                as DescriptionAttribute;
            return attribute?.Description;
        }
    }
}
