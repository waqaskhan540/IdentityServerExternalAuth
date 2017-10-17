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

        public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser:IdentityUser,new()
        {
            services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor<TUser>>();
            services.AddScoped<IEmailUserProcessor, EmailUserProcessor<TUser>>();
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant<TUser>>();
            services.AddSingleton<HttpClient>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
                  
            services.AddScoped<IProviderRepository, ProviderRepository>();
            return services;
        }

        public static IServiceCollection AddProviders<TUser>(this IServiceCollection services) where TUser: IdentityUser,new()
        {
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider<TUser>>();
            services.AddTransient<ITwitterAuthProvider, TwitterAuthProvider<TUser>>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<TUser>>();
            services.AddTransient<ILinkedInAuthProvider, LinkedInAuthProvider<TUser>>();
            services.AddTransient<IGitHubAuthProvider, GitHubAuthProvider<TUser>>();
            return services;
        }
    }
}
