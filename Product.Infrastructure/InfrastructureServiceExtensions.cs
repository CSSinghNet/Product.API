using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName)));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
