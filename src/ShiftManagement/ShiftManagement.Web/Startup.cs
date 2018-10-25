using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace ShiftManagement.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Identity;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Routing;
    using Claims;
    using AutoMapper;
    using Newtonsoft.Json;
    using DataAccess;
    using Infrastructure;
    using DataAccess.Interfaces;
    using DataAccess.Repositories;
    using Domain;
    using Services.Interfaces;
    using Services.Employees;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Data;
    using Mapper;

    public class Startup
    {
        private const string DataAccessAssemblyName = "ShiftManagement.DataAccess";
        private const string DefaultConnectionName = "ConnectionStrings:DefaultConnection";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }); 

            AspNetIdentityRegistration(services);
            RegisterJwtTokenAuthentication(services);
            IocRegistration(services);
            RegisterSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataSeeder dataSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            this.HostingEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // Disable caching for all static files.
                    context.Context.Response.Headers["Cache-Control"] = Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] = Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] = Configuration["StaticFiles:Headers:Expires"];
                }
            });
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Shiftmanagement API v1");

            });

            app.UseAuthentication();
            app.UseJwtProvider(this.ServiceProvider);
            app.UseMvc(routes => BuildRoutes(routes));
            app.UseCors(cfg => 
            {
                cfg.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        }
        
        private void BuildRoutes(IRouteBuilder rootBuilder)
        {
            rootBuilder.MapRoute(
                    name: "default",
                    template: "{controller}/{action=index}/{id}");
        }

        private void AspNetIdentityRegistration(IServiceCollection services)
        {
            var connection = Configuration[DefaultConnectionName];
            services.AddIdentity<Employee, EmployeeRole>(ConfigurationIdentity)
                    .AddEntityFrameworkStores<ShiftManagementDbContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext<ShiftManagementDbContext>(o =>
            {
                o.UseSqlServer(connection, b => b.MigrationsAssembly(DataAccessAssemblyName));
            });
        }

        private void RegisterJwtTokenAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(JwtTokenProvider.SecurityKey.Key),
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

        }

        private void ConfigurationIdentity(IdentityOptions config)
        {
            config.User.RequireUniqueEmail = true;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequiredLength = 6;
        }

        private void IocRegistration(IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddSingleton(this.Configuration);
            services.AddTransient<DataSeeder>();
            services.AddTransient<IObjectFactory, ShiftManagement.Infrastructure.ObjectFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            ServiceProvider = services.BuildServiceProvider();
            services.AddSingleton(ServiceProvider);
            services.AddAutoMapper(cfg => 
            {
                cfg.AddProfile<MapperProfile>();
            });
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shiftmanagement API", Version = "v1" });
            });
        }
    }
}
