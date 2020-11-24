using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace LegoAccounting.Web.Utils
{
	/// <summary>
	/// Model builder used to validate ObjectId FromQuery, FromRoute, etc.
	/// </summary>
	public class ObjectIdModelBinder : IModelBinder
	{
		// ToDo: handle exceptions like https://docs.microsoft.com/ru-ru/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-3.1
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			string rawData = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

			try
			{
				// Manually parsing value
				var result = ObjectId.Parse(rawData);

				bindingContext.Result = ModelBindingResult.Success(result);
			}
			catch (Exception ex)
			{
				throw;
			}

			return Task.CompletedTask;
		}
	}
}
