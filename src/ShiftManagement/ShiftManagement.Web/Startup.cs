namespace ShiftManagement.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ShiftManagement.Infrastructure;
    using ShiftManagement.Services.Interfaces;
    using ShiftManagement.Services.Employees;
    using System;
    using ShiftManagement.DataAccess.Repositories;
    using ShiftManagement.DataAccess.Interfaces;
    using ShiftManagement.Domain;
    using ShiftManagement.DataAccess;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IServiceProvider ServiceProvider { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            IocRegistration(services);
            AspNetIdentityRegistration(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private void AspNetIdentityRegistration(IServiceCollection services)
        {
            var connection = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ShiftManagementDbContext>(o =>
                    o.UseSqlServer(connection, b => b.MigrationsAssembly("ShiftManagement.DataAccess")));

            services.AddIdentity<Employee, EmployeeRole>()
                    .AddEntityFrameworkStores<ShiftManagementDbContext, int>()
                    .AddDefaultTokenProviders();
        }

        private void IocRegistration(IServiceCollection services)
        {
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IObjectFactory, Infrastructure.ObjectFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            ServiceProvider = services.BuildServiceProvider();
            services.AddSingleton(ServiceProvider);
        }

    }
}
