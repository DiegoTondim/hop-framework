using Hop.Api.Server.Core;
using Hop.Framework.Core;
using Hop.Framework.Core.User;
using Hop.Framework.Domain;
using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Validation;
using Hop.Framework.EFCore.Context;
using Hop.Net5WebApi.Application.Services;
using Hop.Net5WebApi.Domain.Commands;
using Hop.Net5WebApi.Domain.Entities;
using Hop.Net5WebApi.Domain.Validators;
using Hop.Net5WebApi.Infra.Contexts;
using Hop.Net5WebApi.Infra.Repositories;
using Hop.Net5WebApi.Infra.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Hop.Net5WebApi
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
            services
                .AddDomainNotifications()
                .AddUserContextService()
                .AddDispatcher()
                .AddScoped<IPersonService, PersonService>()
                .AddScoped<IValidation<RegisterNewPersonCommand>, RegisterNewPersonValidator>()
                .AddScoped<IValidation<UpdatePersonCommand>, UpdatePersonValidator>()
                .AddScoped<IUnitOfWork, InMemoryUoW>()
                .AddScoped<IRepositoryWithGuidKey<PersonEntity>, PersonRepository>()
                .AddSingleton<HopContextBase, DbContext>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hop.Net5WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hop.Net5WebApi v1"));
            }

            app.UseHopErrorHandlerMiddleware();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
