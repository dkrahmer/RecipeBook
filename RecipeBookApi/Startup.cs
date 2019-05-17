using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Common.Dynamo;
using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBookApi.Options;
using RecipeBookApi.Services;
using RecipeBookApi.Services.Contracts;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

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

            services.Configure<AppCorsOptions>(_configuration.GetSection("Cors"));
            services.Configure<AppGoogleOptions>(_configuration.GetSection("Authentication:Google"));
            services.Configure<AppDateOptions>(_configuration.GetSection("DateSettings"));
            
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "Recipe Book API", Version = "v1" });
            });

            services.AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    var appCorsOptions = services.BuildServiceProvider().GetService<IOptions<AppCorsOptions>>();

                    corsPolicyBuilder.WithOrigins(appCorsOptions.Value.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    var appGoogleOptions = services.BuildServiceProvider().GetService<IOptions<AppGoogleOptions>>();

                    jwtOptions.RequireHttpsMetadata = false;
                    jwtOptions.SaveToken = true;

                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appGoogleOptions.Value.ClientSecret))
                    };
                });

            services.AddDefaultAWSOptions(_configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();

            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<IDynamoStorageRepository<AppUser>, DynamoStorageRepository<AppUser>>();
            services.AddTransient<IDynamoStorageRepository<Recipe>, DynamoStorageRepository<Recipe>>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IAuthService, GoogleAuthService>();
            services.AddTransient<IRecipeService, DynamoRecipeService>();
        }

        public void Configure(IApplicationBuilder app, IOptions<AppCorsOptions> appCorsOptions)
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
            app.UseAuthentication();
            app.UseCors();
            app.UseMvc();
        }
    }
}
