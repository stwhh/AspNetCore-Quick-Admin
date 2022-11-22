using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using QuickAdmin.Common;
using QuickAdmin.Common.Filters;
using QuickAdmin.JWTAuth;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickAdmin.Common.Log;
using QuickAdmin.DAO;
using QuickAdmin.DAO.Repository;
using System.IO;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;

namespace QuickAdmin
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSetting = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSetting>(jwtSetting);

            services.AddControllers(options =>
                {
                    if (HostingEnvironment.IsProduction())
                    {
                        options.Filters.Add<AuditLogActionFilter>(); //生产环境启用审计日志
                    }
                })
                .AddControllersAsServices() //通过容器创建Controller 详见：https://autofaccn.readthedocs.io/zh/latest/integration/aspnetcore.html#asp-net-core-3-0-and-generic-hosting
                .AddNewtonsoftJson(options => //添加NewtonsoftJson作为序列化的库
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //Newtonsoft.Json默认不会修改首字母为小写的。System.Text.Json序列化默认是首字母小写的
                    //序列化首字母小写
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
                    { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                });

            var dbType = Configuration.GetValue<string>("DataBaseType").ToLower();
            if (dbType == DbTypeEnum.SqlServer.ToString().ToLower())
            {
                services.AddDbContextPool<QuickAdminDbContext>(
                    options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            }
            else if (dbType == DbTypeEnum.MySql.ToString().ToLower())
            {
	            services.AddDbContextPool<QuickAdminDbContext>(
		            options =>
		            {
			            options.UseMySql(Configuration.GetConnectionString("MySql"),
				            ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql"))); //new MySqlServerVersion(new Version(8, 0, 21))
                    });
            }
            else if (dbType == DbTypeEnum.PgSql.ToString().ToLower())
            {
                services.AddDbContextPool<QuickAdminDbContext>(
                    options => { options.UseNpgsql(Configuration.GetConnectionString("PgSql")); });
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
                    DefaultCorsPolicyName,
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
                        .WithExposedHeaders(
                            "Content-Disposition") //响应头暴露Content-Disposition标识, Return File(ms,"",fileName) 设置的文件名才可以被前端获取到
                )
            );

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "QuickAdmin API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Thomson Sun",
                        Email = string.Empty,
                        Url = new Uri("http://localhost")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("http://localhost")
                    }
                });

                //添加Bearer类型的token验证
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                //给swagger添加注释
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "QuickAdmin.xml");
                options.IncludeXmlComments(xmlPath);
            });

            //添加JWT认证
            services.AddJwtAuthentication(Configuration);
        }

        /// <summary>
        /// Autofac服务注册
        /// Autofac 的服务工厂会在调用 ConfigureServices() 之后，自动调用名为 ConfigureContainer() 的方法
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region 注册services

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray(); Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("QuickAdmin.Services"))
                .PublicOnly()
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //注入仓储
            containerBuilder.RegisterGeneric(typeof(QuickAdminRepository<>)).As(typeof(IQuickAdminRepository<>))
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<LogHelper>().As<ILogHelper>().SingleInstance();
            //containerBuilder.RegisterType<UserService>().As<IUserService>(); //不需要手动注入，上面的代码会自动注入
            containerBuilder.RegisterType<CommonHelper>();

            #endregion
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

            //跨域设置
            //app.UseCors(DefaultCorsPolicyName); // 如果限制跨域，启用这句
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin(); //允许所有来源地址访问
                policy.AllowAnyHeader(); //WithHeaders
                policy.AllowAnyMethod();
                //policy.AllowCredentials(); //跟AllowAnyOrigin()不能共存
                policy.WithExposedHeaders("Content-Disposition"); //响应头暴露Content-Disposition标识, Return File(ms,"",fileName) 设置的文件名才可以被前端获取到
                policy.WithExposedHeaders("token"); //跨域时允许自定义响应头暴露出来，否则前端无法获取token，而且不能为空，否则也取不到
            });

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickAdmin Api V1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //启用认证
            app.UseAuthentication();

            //启用授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
