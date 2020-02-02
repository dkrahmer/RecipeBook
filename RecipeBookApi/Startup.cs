using Common.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBookApi.Models;
using RecipeBookApi.Options;
using RecipeBookApi.Services;
using RecipeBookApi.Services.Contracts;
using Swashbuckle.AspNetCore.Swagger;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
			services.AddMvc(o =>
			{
				o.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None, Duration = 0 }); // disable caching for GET requests
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.Configure<AppOptions>(_configuration);
			var options = services.BuildServiceProvider().GetService<IOptions<AppOptions>>().Value;

			services.AddSwaggerGen(swaggerGenOptions =>
			{
				swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "Recipe Book API", Version = "v1" });
			});

			services.AddCors(corsOptions =>
			{
				corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
				{
					corsPolicyBuilder.WithOrigins(options.AllowedOrigins)
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});

			if (options.DebugMode)
			{
				services.AddMvc(opts =>
				{
					opts.Filters.Add(new AllowAnonymousFilter());
				});
				services.AddAuthentication();
				services.AddTransient<IAuthService, NoAuthService>();
			}
			else
			{
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
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.GoogleClientSecret))
						};
					});

				services.AddTransient<IAuthService, GoogleAuthService>();
			}
			services.AddTransient<IAppUsersService, MySqlAppUsersService>();
			services.AddTransient<IRecipesService, MySqlRecipesService>();

			using (var db = new MySqlDbContext(options.MySqlConnectionString))
			{
				db.Database.EnsureCreated();

				options.SetIngredientUnitStandardizer(db.VolumeToMassConversions);
			}
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
			app.UseAuthentication();
			app.UseCors();
			app.UseMvc();
		}
	}

	internal class NoAuthService : IAuthService
	{
		public Task<string> Authenticate(string token)
		{
			return new Task<string>(() => { return ""; });
		}

		public AppUserClaimModel GetUserFromClaims(ClaimsPrincipal userClaims)
		{
			return new AppUserClaimModel();
		}
	}
}
