using DailyCashFlow.WebApi.Configurations;
using DailyCashFlow.WebApi.Filters;
using Serilog;

namespace DailyCashFlow.WebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true);

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthenticationConfiguration();
			services.AddAuthorization();
			services.AddControllers(options =>
			{
				options.Filters.Add<GlobalExceptionFilter>();
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
