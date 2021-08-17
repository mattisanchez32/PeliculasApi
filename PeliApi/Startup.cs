using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliApi.Helpers;
using PeliApi.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeliApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAutoMapper(typeof(Startup));

			services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivoLocal>();
			services.AddHttpContextAccessor();

			services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

			services.AddScoped<PeliculaExisteAttribute>();

			services.AddSingleton(provider =>

				new MapperConfiguration(Configuration =>
				{
					var geometryFactory = provider.GetRequiredService<GeometryFactory>();
					Configuration.AddProfile(new AutoMapperProfiles(geometryFactory));
				}).CreateMapper()
			);

			services.AddDbContext<AplicationDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
			sqlServerOptions => sqlServerOptions.UseNetTopologySuite()
			));
			services.AddControllers().AddNewtonsoftJson();


			services.AddIdentity<IdentityUser, IdentityRole>()
			 .AddEntityFrameworkStores<AplicationDbContext>()
			 .AddDefaultTokenProviders();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			   .AddJwtBearer(options =>
				   options.TokenValidationParameters = new TokenValidationParameters
				   {
					   ValidateIssuer = false,
					   ValidateAudience = false,
					   ValidateLifetime = true,
					   ValidateIssuerSigningKey = true,
					   IssuerSigningKey = new SymmetricSecurityKey(
				   Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
					   ClockSkew = TimeSpan.Zero
				   }
				);





		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
