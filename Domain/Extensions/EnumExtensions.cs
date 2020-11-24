using System;
using System.Runtime.Serialization;

namespace LegoAccounting.Domain.Extensions
{
	public static class EnumExtensions
	{
		public static T GetValueFromEnumMember<T>(this string value) where T : Enum
		{
			var type = typeof(T);

			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
				
				if (attribute != null)
				{
					if (attribute.Value == value)
						return (T)field.GetValue(null);
				}
				else
				{
					if (field.Name == value)
						return (T)field.GetValue(null);
				}
			}
			throw new ArgumentException($"Unknown value: {value}");
		}

		public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
		{
			var type = value.GetType();
			var memberInfo = type.GetMember(value.ToString());

			if (memberInfo.Length == 0)
				throw new ArgumentException("Wrong Enum member");

			var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

			return attributes.Length > 0 ? (T) attributes[0] : null;
		}

		public static string GetEnumMemberValue(this Enum value)
		{
			return value.GetAttributeOfType<EnumMemberAttribute>()?.Value;
		}
	}
}