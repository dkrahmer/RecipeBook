using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Common.Dynamo;
using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace RecipeBookApi
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info { Title = "Recipe Book API", Version = "v1" });
            });

            var awsOptions = Configuration.GetAWSOptions();

            services.AddScoped<IDynamoDBContext, DynamoDBContext>((s) => {
                var awsAccessKey = Configuration.GetValue<string>("AWSAccessKey");
                var awsSecretAccessKey = Configuration.GetValue<string>("AWSSecretAccessKey");

                return new DynamoDBContext(new AmazonDynamoDBClient(awsAccessKey, awsSecretAccessKey, awsOptions.Region));
            });

            services.AddScoped<IDynamoStorageRepository<Recipe>, DynamoStorageRepository<Recipe>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Book API");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}