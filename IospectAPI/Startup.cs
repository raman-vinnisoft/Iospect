using IospectAPI.Common.Utilities;
using IospectAPI.Repositories.Abstraction.Contracts;
using IospectAPI.Repositories.Context.Context;
using IospectAPI.Repositories.Context.Entities;
using IospectAPI.Repositories.Implementation.Concrete;
using IospectAPI.Services.Abstraction.Contracts;
using IospectAPI.Services.Implementation.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IospectAPI
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
            services.AddHttpContextAccessor();
            services.AddVersionedApiExplorer(opt=>opt.GroupNameFormat="'v'VVV");
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iospect API", Version = "1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Iospect API", Version = "2.0" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);              
                
                c.ResolveConflictingActions(apiDescription => apiDescription.First());
            });
            services.AddApiVersioning(s => { s.DefaultApiVersion = new ApiVersion(1, 0);
                s.AssumeDefaultVersionWhenUnspecified = false;
                s.ReportApiVersions = true;
                s.ApiVersionReader = new HeaderApiVersionReader("version");
            });


            //************************CONTEXT***************************//
            services.AddDbContextFactory<SqlDbContext>(s => s.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Singleton);
            services.AddDbContext<SqlDbContext>(s => s.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Scoped);

            //***********************REPOSITORY*************************//
          
            services.AddScoped<IRepository<BankAccount>, AccountRepository>();
            services.AddScoped<IRepository<Transaction>, TransactionRepository>();


            //************************SERVIES***************************//

            services.AddSingleton<EMailHelper>();
            services.AddScoped<IAccountService, AccountService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(s => {


                s.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
