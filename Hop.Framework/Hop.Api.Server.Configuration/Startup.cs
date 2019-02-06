using Hop.Api.Server.Core.Dispatcher;
using Hop.Api.Server.Core.Settings;
using Hop.Framework.Core.Bootstrapper;
using Hop.Framework.Core.Date;
using Hop.Framework.Core.Host;
using Hop.Framework.Core.IoC;
using Hop.Framework.Core.Messaging;
using Hop.Framework.Core.Messaging.Configuration;
using Hop.Framework.Domain.Module;
using Hop.Framework.NetCoreDI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Net;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Hop.Api.Server.Configuration
{
	public abstract class Startup
	{
		private readonly bool _useMessaging;
		private readonly IIntegrationModule _module;
		private IServiceCollection _serviceConllection;
		private IBootstrapperModules _bootstrapper;
		protected bool DocumentationEnabled = false;
		protected Action<SwaggerGenOptions> SwaggerOptions;
		protected Action<SwaggerUIOptions> SwaggerUIOptions;

		protected Startup(IConfiguration configuration, bool useMessaging = false, IIntegrationModule module = null)
		{
			_useMessaging = useMessaging;
			_module = module;
			Configuration = configuration;
		}

		protected Startup(IHostingEnvironment env, bool useMessaging = false, IIntegrationModule module = null)
		{
			_useMessaging = useMessaging;
			_module = module;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			_serviceConllection = services;
			_bootstrapper = Bootstrapper
				.Configure()
				.UseNetCoreDI(_serviceConllection);
			var mvc = _serviceConllection
				.EnableHopCorsMode()
				.AddMvc(OnAddMvc);
			if (DocumentationEnabled && SwaggerOptions != null)
				_serviceConllection.AddSwaggerGen(SwaggerOptions);

			_serviceConllection.AddScoped<IDateProvider, DateProvider>();
			AddServices(_serviceConllection);
			var register = _bootstrapper
				.UseDomainNotifications()
				.RegisterModule(ConfigureModules);

			_serviceConllection.AddScoped<IDispatcher, Dispatcher>();

			var hostConfiguration = new HostConfiguration(Dns.GetHostName());
			_serviceConllection
				.AddSingleton<IHostConfiguration>((a) => hostConfiguration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			if (DocumentationEnabled && SwaggerUIOptions != null)
			{
				app.UseSwagger();
				app.UseSwaggerUI(SwaggerUIOptions);
			}

			app
				.UseCors("CorsPolicy")
				.UseRequestLocalization()
				.UseMvc()
				.UseHopErrorHandlerMiddleware()
				.UseHopDefaultJsonSettings();

			_bootstrapper.Build();

			if (_useMessaging)
			{
				var provider = ServiceResolver.Container.Resolve<IMessagingConfiguration>();
				ConfigureSubscribers(provider);
				//var messagingProvider = ServiceResolver.Container.Resolve<IMessagingProvider>();
				//messagingProvider.Load(ServiceResolver.Container.Resolve<IProviderConfiguration>(), provider);
			}

			//Registrar evento de shutdown
			applicationLifetime.ApplicationStopping.Register(OnShutdown);

			OnStartup();
		}

		public virtual void ConfigureSubscribers(IMessagingConfiguration configuration)
		{
		}

		public virtual void OnStartup()
		{
			//this code is called when the application starts
		}

		public virtual void OnShutdown()
		{
			//this code is called when the application stops
		}

		protected virtual void AddServices(IServiceCollection services)
		{
			// override this method to add additional services
		}

		protected abstract void OnAddMvc(MvcOptions config);

		public abstract void ConfigureModules(IBootstrapperModulesLoader bootstrapper);
	}
}
