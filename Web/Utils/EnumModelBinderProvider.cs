using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LegoAccounting.Web.Utils
{
	/// <summary>
	/// Custom Model Binder to be able use EnumModelBinder<T> for any Enum whenever Asp.Net tries to parse and bind a model.
	/// </summary>
	public class EnumModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (context.Metadata.ModelType.IsEnum)
			{
				var enumType = typeof(EnumModelBinder<>).MakeGenericType(context.Metadata.ModelType);

				return Activator.CreateInstance(enumType) as IModelBinder;
			}

			return null;
		}
	}
}