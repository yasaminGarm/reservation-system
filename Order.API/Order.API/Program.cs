
using BeanSceneWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Order.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            var cs = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));

            builder.Services.AddCors(options =>
            {
                //build a defsault cors policy
                options.AddDefaultPolicy(policy =>
                {
                    //allow any origion to acccess ourAPI
                    policy.AllowAnyOrigin();
                    //allow specific originis to access
                    //policy.WithOrigins("url", "url");
                    //alllow any http header
                    policy.AllowAnyHeader();
                    //alllow any http method
                    policy.AllowAnyMethod();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

            app.UseCors();

            app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}