namespace LegoAccounting.Web.Utils
{
	//ToDo: use this in case of switching to System.Text.Json.Serialization
	/*services.AddControllers().AddJsonOptions(options =>
	{
		// Use the default property (Pascal) casing.
		//options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

		// Configure a custom converter.
		options.JsonSerializerOptions.Converters.Add(new EnumJsonConverter());
		options.JsonSerializerOptions.Converters.Add(new StateTypeJsonConverter());
	});*/

	/*public class EnumJsonConverter : JsonConverter<Enum>
	{
		public override Enum Read(ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			var str = reader.GetString();



			return StateType.Used;
		}

		public override void Write(Utf8JsonWriter writer,
			Enum value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue("U");
		}
	}

	public class StateTypeJsonConverter : JsonConverter<StateType>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == typeof(StateType);
		}

		public override StateType Read(ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			var str = reader.GetString();

			return StateType.Used;
		}

		public override void Write(Utf8JsonWriter writer,
			StateType value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue("U");
		}
	}*/
	/*public static class EnumExtensions
	{
		public static T GetValueFromEnumMember<T>(this string value)
		{
			var type = typeof(T);
			if (!type.IsEnum) throw new InvalidOperationException();
			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(EnumMemberAttribute)) as EnumMemberAttribute;
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
	}*/
}