using LegoAccounting.Web.Extensions;
using LegoAccounting.Web.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegoAccounting.Web
{
	public class Startup
	{
		readonly string CorsOrigins = "default";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(name: CorsOrigins,
					builder =>
					{
						builder.AllowAnyHeader().WithOrigins("http://localhost:3000");
						//builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
					});
			});

			services
				.AddControllers(options =>
				{
					options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider());
					options.ModelBinderProviders.Insert(1, new ObjectIdModelBinderProvider());
				})
				.AddNewtonsoftJson();

			services.AddConfiguration(Configuration);
			services.AddDatabase();
			services.AddRepositories();
			services.AddIntegrationServices();
			services.AddServices();

			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/build";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseRouting();

			app.UseCors(CorsOrigins);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "api",
					pattern: "api/{controller}/{action}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseProxyToSpaDevelopmentServer($"http://localhost:3000");
					//spa.UseReactDevelopmentServer(npmScript: "start");
				}
			});
		}
	}
}
