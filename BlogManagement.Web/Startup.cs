using BlogManagement.DataAccess;
using BlogManagement.DataAccess.Abstract;
using BlogManagement.DataAccess.Repositories;
using BlogManagement.Infrastructure;
using BlogManagement.Infrastructure.Abstract;
using BlogManagement.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlogManagement.DataAccess.Profiles;
using BlogManagement.Infrastructure.Extensions;
using BlogManagement.Infrastructure.Validation;
using BlogManagement.Services;
using BlogManagement.Validation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace BlogManagement
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
            services.AddDbContext<BlogDbContext>(opt =>
            {
                opt.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery) )
                    .EnableSensitiveDataLogging();
            });

            services.Configure<JwtConfigOptions>(Configuration.GetSection("JwtConfig"));
            services.Configure<RedisConfigurationOptions>(Configuration.GetSection("Redis"));
            
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<JwtConfigOptions>,JwtConfigValidation>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<RedisConfigurationOptions>,RedisConfigValidation>());

            services.AddAuthenticationWithJwtBearerToken(Configuration);

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<BlogDbContext>();

            services.AddDatabaseDeveloperPageExceptionFilter();
            
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });

            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IBlogService, BlogService>();
            services.Decorate<IBlogService, CachedBlogService>();

            services.Scan(scan =>
                scan.FromAssemblyOf<IPostValidator>()
                    .AddClasses(c => c.AssignableTo<IPostValidator>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            
            services.AddScoped<IPostValidationProcessor, DefaultPostValidationProcessor>();

            services.AddSwaggerGen();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetRedisConfiguration();
            });
            
            services.AddAutoMapper(typeof(PostProfile));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogAPI V1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}