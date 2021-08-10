using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Helpers
{
	//tiene metodos de extension por eso es estatica
	public static class HttpContextExtension
	{
		//el Iqueryable es el q sirve para determinar la cantidad de registros en la tabla
		public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext,
			IQueryable<T> queryable, int cantidadRegistrosPorPagina)
		{
			double cantidad = await queryable.CountAsync();
			double cantidadPorPagina = Math.Ceiling(cantidad / cantidadRegistrosPorPagina);
			httpContext.Response.Headers.Add("cantidadPaginas", cantidadPorPagina.ToString());

		}
			
	}
}
