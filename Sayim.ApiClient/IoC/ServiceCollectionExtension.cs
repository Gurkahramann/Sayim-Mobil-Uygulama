using Microsoft.Extensions.DependencyInjection;
using Sayim.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sayim.ApiClient.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApiClientService(this IServiceCollection services,Action<ApiClientOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<ApiClientService>();
            return services;
        }
    }
}
