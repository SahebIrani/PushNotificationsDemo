
using AngularSignalR.Hubs;

using Demo.AspNetCore.Angular.PushNotifications.Services;

using Lib.Net.Http.WebPush;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SImple.Data;
using SImple.Models;

using WebPush;

namespace SImple
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
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddAuthentication().AddIdentityServerJwt();

			services.AddControllersWithViews();
			services.AddRazorPages();

			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});

			services.AddSignalR();

			var vapidDetails = new VapidDetails(
				Configuration.GetValue<string>("VapidDetails:Subject"),
				Configuration.GetValue<string>("VapidDetails:PublicKey"),
				Configuration.GetValue<string>("VapidDetails:PrivateKey"))
			;
			services.AddTransient(c => vapidDetails);

			services.AddCors(options =>
			{
				options.AddPolicy("PushNotificationsCorsPolicy", builder =>
					builder
						   .AllowAnyMethod()
						   .AllowAnyHeader()
						   .AllowCredentials()
						   .AllowAnyOrigin()
						   .WithOrigins(
							 "http://localhost:8031",
							 "https://localhost:8031",
							 "http://localhost:5000",
							 "https://localhost:5001")
						   .Build());
			});

			services.Configure<PushNotificationsOptions>(Configuration.GetSection("PushNotifications"));
			services.AddSingleton<IPushSubscriptionsService, PushSubscriptionsService>();
			services.AddHttpClient<PushServiceClient>();
			services.AddHostedService<WeatherNotificationsProducer>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}

			app.UseRouting();

			app.UseCors("PushNotificationsCorsPolicy");

			app.UseAuthentication();
			app.UseIdentityServer();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
				endpoints.MapRazorPages();

				endpoints.MapHub<DrinkingHub>("/drinking");
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});
		}
	}
}
