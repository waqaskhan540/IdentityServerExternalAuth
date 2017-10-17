using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IdentityServerExternalAuth.Extensions;
using System;
using IdentityServerExternalAuth.Entities;

namespace IdentityServerExternalAuth
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDatabaseConfiguration(Configuration.GetConnectionString("DefaultConnection"))
                    .AddIdentityServerConfig()
                    .AddServices<ApplicationUser>()
                    .AddRepositories()
                    .AddProviders<ApplicationUser>();

            services.AddMvc();
        }



        public IConfiguration Configuration;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
