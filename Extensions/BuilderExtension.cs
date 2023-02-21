using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Minio.AspNetCore;

using SWP391.Project.DbContexts;
using SWP391.Project.Models;
using SWP391.Project.Repositories;
using SWP391.Project.Services;

namespace SWP391.Project.Extensions
{
    public static class BuilderExtension
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;
            ConfigurationManager configuration = builder.Configuration;

            _ = services.Configure<JWTModel>(
                config: configuration.GetSection("JWT"));

            _ = services.AddControllers().AddJsonOptions(configure: conf => conf.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            _ = services.AddDbContextPool<ProjectDbContext>(
                optionsAction: builder =>
                    builder.UseSqlServer(
                                connectionString:
                                    configuration.GetConnectionString("Development"),
                                sqlServerOptionsAction: options =>
                                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
                poolSize: 128);

            _ = services.AddMinio(configure: options =>
            {
                options.Endpoint = configuration["MinIO:Endpoint"]!;
                options.AccessKey = configuration["MinIO:AccessKey"]!;
                options.SecretKey = configuration["MinIO:SecretKey"]!;
            });

            _ = services.AddAutoMapper(assemblies: AppDomain.CurrentDomain.GetAssemblies());

            _ = services.AddProductServices();

            _ = services.AddEndpointsApiExplorer();

            _ = services.AddSwaggerGen(setupAction: options =>
                {
                    options.SwaggerDoc(
                    name: "v1",
                    info: new()
                    {
                        Version = "v1",
                        Title = "SWP391",
                        Description = string.Empty,
                    });

                    options.AddSecurityDefinition(
                        name: "JWT",
                        securityScheme: new()
                        {
                            Description = "JWT Authorization header using the Bearer scheme.",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = "Bearer"
                        });

                    options.AddSecurityRequirement(
                        securityRequirement: new()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "JWT"
                                    }
                                },
                                new List<string>()
                            }
                        });

                    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                });

            _ = services
                .AddAuthentication(
                    configureOptions: options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    configureOptions: options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new()
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                        };
                    });

            _ = services.AddAuthorization();

            _ = services
                .AddCors(setupAction: options =>
                    options
                        .AddPolicy(
                            name: "CorsPolicy",
                            configurePolicy: builder =>
                                builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));
            return builder;
        }

        private static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            // repositories
            _ = services.AddScoped<IAccountRepository, AccountRepository>();
            _ = services.AddScoped<IAddressRepository, AddressRepository>();
            _ = services.AddScoped<IBrandRepository, BrandRepository>();
            _ = services.AddScoped<ICartRepository, CartRepository>();
            _ = services.AddScoped<IColorRepository, ColorRepository>();
            _ = services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            _ = services.AddScoped<IImageRepository, ImageRepository>();
            _ = services.AddScoped<IMinioRepository, MinioRepository>();
            _ = services.AddScoped<IOrderRepository, OrderRepository>();
            _ = services.AddScoped<IProductRepository, ProductRepository>();
            _ = services.AddScoped<IProductInStockRepository, ProductInStockRepository>();
            _ = services.AddScoped<ISizeRepository, SizeRepository>();

            // services
            _ = services.AddScoped<IAuthService, AuthService>();
            _ = services.AddScoped<IAccountService, AccountService>();
            _ = services.AddScoped<ICartService, CartService>();
            _ = services.AddScoped<IOrderService, OrderService>();
            _ = services.AddScoped<IProductService, ProductService>();
            _ = services.AddScoped<IStatisticService, StatisticService>();

            return services;
        }
    }
}