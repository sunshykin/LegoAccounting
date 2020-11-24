using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace LegoAccounting.Web.Utils
{
	/// <summary>
	/// Custom Model Binder to be able use ObjectId whenever Asp.Net tries to parse and bind a model.
	/// </summary>
	public class ObjectIdModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			return context.Metadata.ModelType == typeof(ObjectId)
				? new ObjectIdModelBinder()
				: null;
		}
	}
}