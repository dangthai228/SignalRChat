
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Hubs;
using SignalR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.Entities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SignalR
{
    public class Startup
    {
        
        //  https://localhost:44347
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSignalR();
            services.AddRazorPages();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44377")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) )
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero

                    };
                });
            
            services.AddControllers();
           
            // configure strongly typed settings object
            
            // configure DI for application services
            services.AddScoped<IUserService, UserServices>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<MessageHub>("/hubs/MessHub");

            });
        }
    }

    
}
