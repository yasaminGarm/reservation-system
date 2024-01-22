global using Xunit;
using BeanSceneWebApp.Data;
using BeanSceneWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BeanScene.WebApp.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding in-memory database
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), b => b.EnableNullChecks(false)));


            services.AddScoped<PersonService>();

            services.AddTransient<SittingServices>();
            services.AddTransient<ReservationService>();

            services.AddAutoMapper(typeof(Startup));

            //services.AddAuthorization();

            


        }
    }
}