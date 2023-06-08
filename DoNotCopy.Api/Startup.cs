using DoNotCopy.Api.Services;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Entities.Identity;
using DoNotCopy.Core.Infrastructure;
using DoNotCopy.Core.Services;
using DoNotCopy.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api
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
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection"),
                  x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddDefaultTokenProviders();

        services.AddMemoryCache();
           services.Configure<GeneralSettings>(Configuration.GetSection("GeneralSettings"));

            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
            services.AddHttpClient();
            services.AddControllers();
            services.AddSingleton<IImageUploader, LocalImageUploader>();
            services.AddTransient<ClearFolderService>();
            services.AddTransient<EncryptionService>();
            services.AddTransient<FileService>();
            services.AddSingleton<IPathProvider, PathProvider>();
            services.AddTransient<GeneralSettings>();

            var emailConfig = Configuration
               .GetSection("EmailConfiguration")
               .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //file

            var fileFolders = new ReadOnlyCollection<string>(new[]
{
            Configuration["GeneralSettings:UploadPath"],
            Configuration["GeneralSettings:TempltePath"],
            Configuration["GeneralSettings:ResultTempltePath"],
                        Configuration["GeneralSettings:ReportPath"],
        });
            foreach (var fileFolder in fileFolders)
            {
                string filePath = Path.Combine(Configuration["GeneralSettings:FileStoragePath"], fileFolder);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(filePath),
                    RequestPath = new PathString("/" + fileFolder),
                    EnableDirectoryBrowsing = false,
                    //StaticFileOptions =
                    //{
                    //    ContentTypeProvider = new CustomFileExtensionContentTypeProvider(),
                    //}
                });
            }
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
