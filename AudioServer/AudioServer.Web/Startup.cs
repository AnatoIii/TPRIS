using AudioServer.DataAccess;
using AudioServer.Models;
using AudioServer.Service;
using AudioServer.Service.HelperFunctions;
using AudioServer.Service.Interfaces;
using AudioServer.Services;
using AudioServer.Services.Interfaces;
using AudioServer.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;

namespace AudioServer.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<Hosts>(Configuration.GetSection("Hosts"));
            services.Configure<TokenConfig>(Configuration.GetSection("TokenConfig"));
            services.AddDbContext<AudioServerDBContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("AudioServer")).EnableSensitiveDataLogging();
            });
            Configuration.GetSection("ConnectionString");

            services.AddHttpClient<FileServerClient>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IFileServerClient, FileServerClient>();
            services.AddSingleton<TokenHelpers>();

            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("TokenConfig")["Secret"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddHttpContextAccessor();

            services.AddMvc();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder
                    //.WithOrigins("http://*")
                    .AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(options =>
            {
                options.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials(); // allow credentials
            });

            app.UseHttpsRedirection();

            app.UseExceptionHandlingMiddleware();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}