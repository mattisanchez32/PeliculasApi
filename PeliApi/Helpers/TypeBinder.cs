using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Helpers
{
	//Sirve para enviar la lista de enteros del id de generos por ejemplo para las peliculas 
	public class TypeBinder<T> : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var nombrePropiedad = bindingContext.ModelName;
			var proveedorDeValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);

			if(proveedorDeValores == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			try
			{
				var valorDeserializado = JsonConvert.DeserializeObject<T>(proveedorDeValores.FirstValue);
				bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
			}
			catch
			{
				bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor invalido para tipo listado de enteros");

			}

			return Task.CompletedTask;
		}
	}
}
