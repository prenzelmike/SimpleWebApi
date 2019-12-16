using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.HttpOverrides;

using SimpleWebApi.Models;
using SimpleWebApi.Services;


namespace SimpleWebApi
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => 
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    //specify either explictly which methods to allow or use .AllowAnyMethod
                    .WithMethods("GET", "PUT", "DELETE", "POST")
                    .WithHeaders(HeaderNames.AccessControlAllowHeaders, HeaderNames.ContentType);
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<CookbookMongoDbSettings>(
                Configuration.GetSection(nameof(CookbookMongoDbSettings))
            );

            services.AddSingleton<ICookbookMongoDbSettings>(sp => sp.GetRequiredService<IOptions<CookbookMongoDbSettings>>().Value);

            services.AddSingleton<ReceipeService>();

            services.AddControllers()
                .AddNewtonsoftJson(Options=>Options.UseCamelCasing(true));
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => 
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience ="api1";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions{
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

            // new with .netcore 3.0, after updating from 2.2 routes where not found anymore until
            // app.UseRouting() and app.UseEndpoints() where introduced.
            // see https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.endpointroutingapplicationbuilderextensions.userouting?view=aspnetcore-3.0
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //marked as obsolete for endpoint routing app after upgrading to .NetCore 3.0.1
            //app.UseMvc();
            
        }
    }
}