using DataService.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryExtensions
    {

        public static void ConfigureRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
      
        }

    }
    }
