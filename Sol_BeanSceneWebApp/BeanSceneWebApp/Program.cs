using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Globalization;
using System.Text;


namespace BeanSceneWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ////CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-AU");
            ////Thread.CurrentThread.CurrentUICulture = newCulture;
            ////// Make current UI culture consistent with current culture.
            ////Thread.CurrentThread.CurrentCulture = newCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var cs = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


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

            builder.Services.AddScoped<PersonService>();

            builder.Services.AddTransient<SittingServices>();
            builder.Services.AddTransient<ReservationService>();

            builder.Services.AddControllersWithViews().AddViewOptions(o =>
            {
                o.HtmlHelperOptions.ClientValidationEnabled = builder.Configuration.GetSection("AppSettings").GetValue<bool>("ClientValidationEnabled");
            });
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = "JWT_OR_COOKIE";
                o.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,

                           ValidIssuer = builder.Configuration["Jwt:Issuer"],
                           ValidAudience = builder.Configuration["Jwt:Audience"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                           // Prevents tokens without an expiry from ever working, as that would be a security vulnerability.
                           RequireExpirationTime = true,

                           // ClockSkew generally exists to account for potential clock difference between issuer and consumer
                           // But we are both, so we don't need to account for it.
                           // For all intents and purposes, this is optional
                           ClockSkew = TimeSpan.Zero
                       };
                   })
                   .AddPolicyScheme("JWT_OR_COOKIE", null, o =>
                   {
                       o.ForwardDefaultSelector = c =>
                       {
                           string auth = c.Request.Headers[HeaderNames.Authorization];
                           if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer "))
                           {
                               return JwtBearerDefaults.AuthenticationScheme;
                           }

                           return IdentityConstants.ApplicationScheme;
                       };
                   });

            //builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(

                   name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                      );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.MapRazorPages();

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}