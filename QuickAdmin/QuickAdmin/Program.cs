using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using QuickAdmin.Common;
using QuickAdmin.Common.Filters;
using QuickAdmin.Common.Log;
using QuickAdmin.DAO;
using QuickAdmin.DAO.Repository;
using QuickAdmin.JWTAuth;

const string defaultCorsPolicyName = "localhost";
var builder = WebApplication.CreateBuilder(args);

//Autofac 服务注册
//详见：https://learn.microsoft.com/zh-cn/aspnet/core/migration/50-to-60?view=aspnetcore-6.0&tabs=visual-studio#smhm
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

#region 服务注册相关，向容器中添加服务
//JWT 配置
var jwtSetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSetting>(jwtSetting);

builder.Services.AddControllers(options =>
    {
        if (builder.Environment.IsProduction())
        {
            options.Filters.Add<AuditLogActionFilter>(); //生产环境启用审计日志
        }
    })
    //.AddControllersAsServices()
    .AddNewtonsoftJson(options => //添加NewtonsoftJson作为序列化的库
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        //序列化首字母小写,Newtonsoft.Json默认不会修改首字母为小写的。System.Text.Json序列化默认是首字母小写的
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
            { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
    });
builder.Services.AddEndpointsApiExplorer();

//注册数据库 DbContext
var dbType = builder.Configuration.GetValue<string>("DataBaseType").ToLower();
if (dbType == DbTypeEnum.SqlServer.ToString().ToLower())
{
    builder.Services.AddDbContextPool<QuickAdminDbContext>(
        options => { options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")); });
}
else if (dbType == DbTypeEnum.MySql.ToString().ToLower())
{
    builder.Services.AddDbContextPool<QuickAdminDbContext>(
        options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))); //new MySqlServerVersion(new Version(8, 0, 21))
        });
}
else if (dbType == DbTypeEnum.Oracle.ToString().ToLower())
{
	builder.Services.AddDbContextPool<QuickAdminDbContext>(
		options => { options.UseOracle(builder.Configuration.GetConnectionString("Oracle")); });
}
else if (dbType == DbTypeEnum.PgSql.ToString().ToLower())
{
    builder.Services.AddDbContextPool<QuickAdminDbContext>(
        options => { options.UseNpgsql(builder.Configuration.GetConnectionString("PgSql")); });
}

//注入HttpContext服务，否则用不了HttpContextAccessor
builder.Services.AddHttpContextAccessor();

//其它反向代理服务器 启用转接头,保证可以正确获取客户端的信息
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

//Configure CORS for web application
builder.Services.AddCors(
    options => options.AddPolicy(
        defaultCorsPolicyName,
        corsBuilder => corsBuilder
            .WithOrigins( // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                builder.Configuration["App:CorsOrigins"]
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
builder.Services.AddSwaggerGen(options =>
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
builder.Services.AddJwtAuthentication(builder.Configuration);

//构建WebApplication，所有的service 注入必须在这之前申明
var app = builder.Build();

#endregion


#region 配置中间件
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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

app.MapControllers();

app.Run();

#endregion


//Autofac服务注册
void ConfigureContainer(ContainerBuilder containerBuilder)
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
