﻿using BufTools.DI.ReflectionHelpers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Domain
{
    /// <summary>
    /// Class to register Domain related services
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Adds domain related dependencies to the service collection
        /// </summary>
        /// <param name="services">The service collection to add to</param>
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingletonClasses<IValidator>(typeof(ServiceRegistration).Assembly);
            return services;
        }
    }
}
