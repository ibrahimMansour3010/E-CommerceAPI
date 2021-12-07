using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models.Customer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class StartUp
    {
        public IConfiguration Configuration { get; }
        public StartUp(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection service)
        {
            service.AddControllers();
            service.AddSwaggerGen();
            service.AddDbContext<Context>(
                options =>
                {
                    options.UseSqlServer(this.Configuration
                                             .GetConnectionString("ECommerceSTR"));
                }
                );
            service.AddTransient(typeof(IMainRepository<>), typeof(MainRepository<>));
            service.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            service.AddIdentity<ApplicationUserEntity, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();
            service.AddTransient<IAppUserRepository, AppUserRepository>();
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudiene"],
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            service.AddAuthorization(options =>
            {
                options.AddPolicy("adminPolicy", police =>
                {
                    police.RequireRole("Admin");
                });
            });
           
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endPoint =>
            {
                endPoint.MapControllers();
            });
        }
    }
}
