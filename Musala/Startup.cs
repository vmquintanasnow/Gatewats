using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Linq;
using AutoMapper;
using Musala.Business.Payload;
using Musala.Business.Mappers;
using Musala.Business.Services;
using Musala.Business.Exceptions;
using Musala.DAL;

namespace Musala
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
            #region Dependency Injection
            services.AddAutoMapper(typeof(GatewayProfile), typeof(DeviceProfile));
            services.AddDbContext<MusalaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IGatewayService, GatewayService>();
            services.AddScoped<IDeviceService, DeviceService>();
            #endregion
            

            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);          

            //services.AddDbContext<MusalaTestContext>(
            //    optionsAction =>
            //    optionsAction.UseMySql("server=10.8.130.250;port=3306;database=prueba;user=root;pwd=root", x => x.ServerVersion("8.0.19-mysql"))
            //    );
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => e.Value.Errors.First().ErrorMessage
                        ).ToArray();

                    var factory = new ErrorPayloadFactory(error: errors, message: "Invalid input.");
                    return new PayloadResult(factory.GetPayload(), HttpStatusCode.BadRequest);
                };
            });

            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Api Docs", Version = "1.0" });
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                }
            );

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
