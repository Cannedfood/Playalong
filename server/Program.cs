using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Util;
using server.Controllers;
using System;

namespace server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Program>())
				.Build()
				.Run();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSpaStaticFiles(options => { options.RootPath = "wwwroot"; });
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				GenerateTypescriptDefinitions("./web/BackendTypes.d.ts", new Type[]{
					typeof(PlayalongApiV1.Song),
					typeof(PlayalongApiV1.SongSection),
				});
				app.UseDeveloperExceptionPage();
			}
			else
			{
				Console.WriteLine("Production!");
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseSpaStaticFiles();
			app.UseSpa(builder => {
				if(env.IsDevelopment()) {
					builder.UseProxyToSpaDevelopmentServer("http://localhost:1234");
				}
			});
		}

		public void GenerateTypescriptDefinitions(string path, Type[] types) {
			System.Console.WriteLine("Generating typescript definitions:");

			using var file = new System.IO.StreamWriter(path, false);
			foreach(var type in types) {
				Console.WriteLine("- " + type.Name);
				type.GenerateTypescriptDefinition(file);
			}
		}
	}
}
