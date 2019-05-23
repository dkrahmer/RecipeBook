using Common.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services;
using RecipeBookApi.Services.Contracts;
using Swashbuckle.AspNetCore.Swagger;

namespace RecipeBookApi
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IHostingEnvironment _currentEnvironment;

		public Startup(IHostingEnvironment env)
		{
			_currentEnvironment = env;

			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(_currentEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{_currentEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables();

			if (_currentEnvironment.IsDevelopment())
			{
				configurationBuilder.AddUserSecrets<Startup>();
			}

			_configuration = configurationBuilder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.Configure<AppOptions>(_configuration);
			var appOptions = services.BuildServiceProvider().GetService<IOptions<AppOptions>>();

			services.AddSwaggerGen(swaggerGenOptions =>
			{
				swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "Recipe Book API", Version = "v1" });
			});

			services.AddCors(corsOptions =>
			{
				corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
				{
					corsPolicyBuilder.WithOrigins(appOptions.Value.AllowedOrigins)
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});

			using (var db = new MySqlDbContext(appOptions.Value.MySqlConnectionString))
			{
				db.Database.EnsureCreated();
			}

			/*
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(jwtOptions =>
				{
					jwtOptions.RequireHttpsMetadata = false;
					jwtOptions.SaveToken = true;

					jwtOptions.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateAudience = false,
						ValidateIssuer = false,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appOptions.Value.GoogleClientSecret))
					};
				});
			*/

			//services.AddTransient<IAuthService, GoogleAuthService>();
			services.AddTransient<IRecipesService, MySqlRecipeService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			if (_currentEnvironment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(swaggerUiOptions =>
			{
				swaggerUiOptions.SwaggerEndpoint($"{(_currentEnvironment.IsDevelopment() ? "" : "/Prod")}/swagger/v1/swagger.json", "Recipe Book API");
			});

			app.UseHsts();
			app.UseHttpsRedirection();
			//app.UseAuthentication();
			app.UseCors();
			app.UseMvc();
		}
	}
}
