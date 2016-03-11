using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using SharpFora.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Authorization;
using SharpFora.Security;
using SharpFora.DAL.Models;
using Microsoft.AspNet.Antiforgery;
using SharpFora.Attributes;

namespace SharpFora
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSharpForaWebSockets();
            services.AddMvc();
            services.AddAntiforgery();
            services.AddCaching();
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<SharpForaContext>();

            services.AddIdentity<User, Role>()
                .AddTokenProvider<TOTPTokenProvider>("TwoFactorProvider")
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Login", policy =>
                {
                    policy.Requirements.Add(new LoginRequirement());
                });
            });

            services.ConfigureAntiforgery(options =>
            {
                options.CookieName = "XSRF";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseRuntimeInfoPage();
            }

            app.UseSharpForaWebSockets();
            app.UseIdentity()
                .UseIISPlatformHandler()
                .UseStaticFiles()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });


            // Static 304 caching
            var cache = app.ApplicationServices.GetRequiredService<IMemoryCache>();
            cache.Set("ServerStart", (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond).ToString());

            // Database migration
            var dbContext = app.ApplicationServices.GetRequiredService<SharpForaContext>();
            dbContext.Database.Migrate();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
