using IdentityServer4.Validation;
using IdentityServerExternalAuth.Configuration;
using IdentityServerExternalAuth.Data;
using IdentityServerExternalAuth.Entities;
using IdentityServerExternalAuth.ExtensionGrant;
using IdentityServerExternalAuth.Interfaces;
using IdentityServerExternalAuth.Interfaces.Processors;
using IdentityServerExternalAuth.Processors;
using IdentityServerExternalAuth.Providers;
using IdentityServerExternalAuth.Repositories;
using IdentityServerExternalAuth.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<DatabaseContext>(cfg => cfg.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<DatabaseContext>()
                  .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddIdentityServerConfig(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
               .AddInMemoryApiResources(Config.GetApiResources())
               .AddInMemoryClients(Config.GetClients())
               .AddInMemoryIdentityResources(Config.GetIdentityResources())
               .AddAspNetIdentity<ApplicationUser>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor>();
            services.AddScoped<IEmailUserProcessor, EmailUserProcessor>();
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant>();
            services.AddSingleton<HttpClient>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
          
            services.AddScoped<IExternalUserRepository, ExternalUserRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider>();
            services.AddTransient<ITwitterAuthProvider, TwitterAuthProvider>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
            services.AddTransient<ILinkedInAuthProvider, LinkedInAuthProvider>();
            return services;
        }
    }
}
