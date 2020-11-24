using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace LegoAccounting.Web.Utils
{
	/// <summary>
	/// Model builder used to validate the Enums FromQuery, FromRoute, etc.
	/// <para> Using values of EnumMemberAttribute.</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EnumModelBinder<T> : IModelBinder where T : Enum
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			string rawData = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
			rawData = JsonConvert.SerializeObject(rawData); //turns value to valid json

			try
			{
				// Manually deserializing value
				var result = JsonConvert.DeserializeObject<T>(rawData);

				bindingContext.Result = ModelBindingResult.Success(result);
			}
			catch (JsonSerializationException ex)
			{
				//do nothing since "failed" result is set by default
			}


			return Task.CompletedTask;
		}
	}
}
