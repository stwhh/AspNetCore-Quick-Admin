using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using QuickAdmin.Common;
using QuickAdmin.Common.Filters;
using QuickAdmin.JWTAuth;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Common.Log;
using DAO;
using DAO.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Imp;
using Services.Interface;
using Swashbuckle.AspNetCore.Swagger;

namespace QuickAdmin
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration,IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var jwtSetting = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSetting>(jwtSetting);

            services.AddMvc(options =>
            {
                if (HostingEnvironment.IsProduction())
                {
                    options.Filters.Add<AuditLogActionFilter>(); //生产环境启用审计日志
                }
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var dbType = Configuration.GetValue<string>("DataBaseType").ToLower();
            if (dbType == DbTypeEnum.SqlServer.ToString().ToLower())
            {
                services.AddDbContextPool<QuickAdminDbContext>(
                    options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            }

            if (dbType == DbTypeEnum.MySql.ToString().ToLower())
            {
                services.AddDbContextPool<QuickAdminDbContext>(
                    options => { options.UseMySql(Configuration.GetConnectionString("MySql")); });
            }

            //注入HttpContext服务，否则用不了HttpContextAccessor
            services.AddHttpContextAccessor();

            //其它反向代理服务器 启用转接头,保证可以正确获取客户端的信息
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
           
            // Configure CORS for web application
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    corsBuilder => corsBuilder
                        .WithOrigins( // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o)
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My Demo API",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "Thomson Sun",
                        Email = string.Empty,
                        Url = "无"
                    },
                    License = new License
                    {
                        Name = "MIT",
                        Url = "无"
                    }
                });

                //添加Bearer类型的token验证
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Input Bearer Token",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Enumerable.Empty<string>()}
                });
            });

            //添加JWT认证
            services.AddJwtAuthentication(Configuration);


            //添加 Autofac
            var containerBuilder = new ContainerBuilder();

            #region 注册services
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray(); Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("QuickAdmin.Services"))
                .PublicOnly()
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //注入仓储
            containerBuilder.RegisterGeneric(typeof(QuickAdminRepository<>)).As(typeof(IQuickAdminRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterType<LogHelper>().As<ILogHelper>().SingleInstance();
            //builder.RegisterType<UserService>().As<IUserService>();
            containerBuilder.RegisterType<CommonHelper>();

            #endregion
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            //跨域设置
            //app.UseCors(_defaultCorsPolicyName); // 如果限制跨域，启用这句
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin(); //允许所有来源地址访问
                policy.AllowAnyHeader(); //WithHeaders
                policy.AllowAnyMethod();
                policy.AllowCredentials();
                policy.WithExposedHeaders("token"); //跨域时允许自定义响应头暴露出来，否则前端无法获取token，而且不能为空，否则也取不到
            });

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //启用认证
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/swagger");

            });
        }
    }
}
