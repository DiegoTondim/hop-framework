using System;
using System.ComponentModel;
using System.Reflection;

namespace Hop.Framework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this int value) where T : struct
        {
            T valorEnum;
            if (Enum.TryParse(value.ToString(), out valorEnum))
                return valorEnum;

            throw new Exception("Cannot convert to enum.");
        }

        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();
            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return element.ToString();
        }

        public static Attribute GetAttribute<Attribute>(this Enum element)
        {
            Type type = element.GetType();
            MemberInfo[] memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(Attribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((Attribute)attributes[0]);
                }
            }

            throw new Exception("Attribute not found.");
        }
    }
}
