using ClientCardTransfer.Data;
using ClientCardTransfer.FileLoader;
using ClientCardTransfer.Repositories;
using ClientCardTransfer.Service;
using ClientCardTransfer.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ClientCardTransfer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            // Регистрация репозиториев и UnitOfWork
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllers();
            services.AddHostedService<WorkService>();//регестрирует и запускает фоновую задачу
            var setting = new Setting();
            Configuration.GetSection(key: "Setting").Bind(setting);
            services.AddSingleton(setting);
            //services.AddSingleton(new TxtToSqlLoader(Configuration.GetConnectionString("DataBaseAddres")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataBaseAddres")));
            services.AddControllers();

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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //  endpoints.MapGet("/", async context =>
            //{
            //  await context.Response.WriteAsync("Hello World!");
            //                    });
            //});
        }
    }
}
