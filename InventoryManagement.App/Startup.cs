using InventoryManagement.App.Context;
using InventoryManagement.DAL.Email;
using InventoryManagement.DAL.Merchant;
using InventoryManagement.DAL.Product;
using InventoryManagement.DAL.User;
using InventoryManagement.DAL.SecurityPrivilege;
using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagement.BLL.Helpers;
using Quartz;
using InventoryManagement.BLL.Interfaces;
using Quartz.Spi;
using Quartz.Impl;
using InventoryManagement.BLL.Interfaces.BLL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagement.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache(options =>
            {
                options.SizeLimit = 4000L * 1024 * 1024; 
            });

            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.AddDbContext<DBContext>(o => o.UseMySQL(Configuration.GetConnectionString("ConnStr")));

            services.AddTransient(typeof(IUserRepository), typeof(UserDLL));
            services.AddTransient(typeof(IEmailRepository), typeof(EmailDLL));
            services.AddTransient(typeof(IMerchantRepository), typeof(MerchantDLL));
            services.AddTransient(typeof(IProductRepository), typeof(ProductDLL));
            services.AddTransient(typeof(ISecurityPrivilegeRepository), typeof(SecurityPrivilegeDLL));

            services.AddAutoMapper(typeof(Startup));

            Globals.APIKey = Configuration.GetSection("AppSettings").GetValue<string>("SendgrAPIKey");
            Globals.FromEmail = Configuration.GetSection("AppSettings").GetValue<string>("FromEmail");
            Globals.Username = Configuration.GetSection("AppSettings").GetValue<string>("Username");
            Globals.HostName = Configuration.GetSection("AppSettings").GetValue<string>("HostName");
            Globals.Password = Configuration.GetSection("AppSettings").GetValue<string>("Password");
            Globals.Port = Configuration.GetSection("AppSettings").GetValue<int>("Port");

            var key = "1234567890 a very long word";
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            services.AddTransient<ICreateEmailHelper, CreateEmailHelper>();
            services.AddTransient<IMailService, SendGridEmailService>();
            services.AddSingleton<EmailHelper>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            
            });

            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(EmailHelper),
                cronExpression: "*/5 * * * * ?")); // run every 5 min

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            InventoryManagementHttpContext.Services = app.ApplicationServices;

            var culture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID)
            {
                DateTimeFormat =
                {
                    ShortDatePattern = "dd/MM/yyyy",
                    LongDatePattern = "dd/MM/yyyy"
                }
            };

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

        }
    }
}
