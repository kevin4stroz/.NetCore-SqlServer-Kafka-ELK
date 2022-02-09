using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using PermissionsN5.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using PermissionsN5.Infraestructure.Repositories;
using PermissionsN5.Core.Interfaces;
using PermissionsN5.Core.Services;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Events;
using PermissionsN5.Infraestructure.External;
using Nest;
using Microsoft.OpenApi.Models;
using Confluent.Kafka;


namespace PermissionsN5.Api
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
            services.AddControllers();

            // logging
            services.AddLogging(logBuilder =>
            {
                // crear logger
                var loggerConfiguration = new LoggerConfiguration()
                                                .MinimumLevel.Information()
                                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                                .MinimumLevel.Override("Microsoft.Hosting.LifeTime", LogEventLevel.Information)
                                                .WriteTo.Console();

                // injectar el servicio
                var logger = loggerConfiguration.CreateLogger();

                // crear configuracion serilog
                logBuilder.Services.AddSingleton<ILoggerFactory>(
                    provider => new SerilogLoggerFactory(logger, dispose: false)    
                );
            });            

            // inyeccion de dependencias string de conexion al contexto
            services.AddDbContext<N5SolucionContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("N5Solucion"))
            );

            // injeccion de dependencias
            // services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPermissionsService, PermissionsService>();
            services.AddTransient<IUtils, Utils>();

            // elasticsearch
            var settings = new ConnectionSettings(new Uri(Configuration["ElasticsearchSettings:uri"]));
            var defaultIndex = Configuration["ElasticsearchSettings:defaultIndex"];
            settings = settings.DefaultIndex(defaultIndex);

            var basicAuthUser = Configuration["ElasticsearchSettings:basicAuthUser"];
            var basicAuthPassword = Configuration["ElasticsearchSettings:basicAuthPassword"];
            settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);

            services.AddSingleton<IElasticClient>(new ElasticClient(settings));

            // kafka
            var producerConfig = new ProducerConfig();
            Configuration.Bind("producer", producerConfig);
            services.AddSingleton<ProducerConfig>(producerConfig);

            // swagger
            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Prueba Tecnica Kevin Astroz - N5 {groupName}",
                    Version = groupName,
                    Description = "Prueba Tecnica Kevin Astroz - N5",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prueba Tecnica Kevin Astroz - N5 V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
