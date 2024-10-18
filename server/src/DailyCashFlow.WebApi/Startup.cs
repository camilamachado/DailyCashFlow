using DailyCashFlow.WebApi.Configurations;
using DailyCashFlow.WebApi.Filters;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Serilog;
using System.Text.Json.Serialization;

namespace DailyCashFlow.WebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			var builder = new ConfigurationBuilder()
				.AddConfiguration(configuration) 
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthenticationConfiguration();
			services.AddAuthorization();
			services.AddControllers(options =>
			{
				options.Filters.Add<GlobalExceptionFilter>();
			})
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			})
			.AddOData(opt =>
			{
				var odataBuilder = new ODataConventionModelBuilder();
				opt.AddODataConfiguration(odataBuilder);
			});

			services.AddDatabaseConfiguration(Configuration);
			services.AddEndpointsApiExplorer();
			services.AddAutoMapperConfiguration();
			services.AddSwaggerConfiguration();
			services.AddDependencyInjectionConfiguration();
			services.AddMediatrConfiguration();
			services.AddFluentValidationConfiguration();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSerilogRequestLogging();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapGet("/", context =>
				{
					context.Response.Redirect("/swagger");
					return Task.CompletedTask;
				});

			});
		}
	}
}
