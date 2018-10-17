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

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            HostingEnvironment = env;
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(opt =>
                {
                    //if (!HostingEnvironment.IsProduction())
                    //{
                    //    opt.SslPort = 44388;
                    //}
                    //opt.Filters.Add(new RequireHttpsAttribute());
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }); 

            AspNetIdentityRegistration(services);
            IocRegistration(services);            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataSeeder dataSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.UseAuthentication();
            //app.UseJwtProvider();

            //// Add the Jwt Bearer Header Authentication to validate Tokens    
            //app.UseJwtBearerAuthentication(new JwtBearerOptions()
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    RequireHttpsMetadata = false,
            //    TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        IssuerSigningKey = JwtTokenProvider.SecurityKey,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Tokens:Issuer"],
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true
            //    }
            //});

            app.UseMvc(routes => BuildRoutes(routes));
            app.UseCors(cfg => 
            {
                cfg.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            SeedDatabase(dataSeeder);
        }
        
        private void BuildRoutes(IRouteBuilder rootBuilder)
        {
            rootBuilder.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
        }

        private void AspNetIdentityRegistration(IServiceCollection services)
        {
            var connection = Configuration[DefaultConnectionName];
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ShiftManagementDbContext>(o =>
                    o.UseSqlServer(connection, b => b.MigrationsAssembly(DataAccessAssemblyName)));

            services.AddIdentity<Employee, EmployeeRole>(config => ConfigurationIdentity(config))
                    .AddEntityFrameworkStores<ShiftManagementDbContext>()
                    .AddDefaultTokenProviders();
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
            services.AddSingleton(this.HostingEnvironment);
            services.AddTransient<DataSeeder>();
            //services.AddTransient<IObjectFactory, DbLoggerCategory.Infrastructure.ObjectFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            ServiceProvider = services.BuildServiceProvider();
            services.AddSingleton(ServiceProvider);
            services.AddAutoMapper(cfg => 
            {
                cfg.AddProfile<MapperProfile>();
            });
        }

        private void SeedDatabase(DataSeeder dataSeeder)
        {
            dataSeeder.SeedAsync().Wait();
        }
    }
}
