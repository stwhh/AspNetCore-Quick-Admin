using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspNetCoreQuickAdmin.Common;
using AspNetCoreQuickAdmin.Common.Filters;
using AspNetCoreQuickAdmin.JWTAuth;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AspNetCoreQuickAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<AuditLogActionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var dbType = Configuration.GetValue<string>("DataBaseType").ToLower();
            if (dbType == DbTypeEnum.SqlServer.ToString().ToLower())
            {
                services.AddDbContextPool<QuickDbContext>(
                    options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            }

            if (dbType == DbTypeEnum.MySql.ToString().ToLower())
            {
                services.AddDbContextPool<QuickDbContext>(
                    options => { options.UseMySql(Configuration.GetConnectionString("MySql")); });
            }

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My Demo API", Version = "v1" ,
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
            var builder = new ContainerBuilder();
            builder.Populate(services);
            #region 注册services

            //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service")) //注册Services
                .AsImplementedInterfaces();
            //属性注入 ，后台可用属性自动注入，不需要构造函数注入了
            builder.RegisterApiControllers(assemblies).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
           
            #endregion

            var container = builder.Build();
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
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader(); //WithHeaders
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
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
