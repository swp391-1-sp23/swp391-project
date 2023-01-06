using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            _ = services.Configure<JWTModel>(config: configuration.GetSection("JWT"));

            _ = services.AddControllers();

            _ = services.AddDbContext<ProjectDbContext>(optionsAction: opts => opts.UseSqlServer(connectionString: configuration.GetConnectionString("Development")));

            _ = services.AddProductServices();

            _ = services.AddEndpointsApiExplorer();

            _ = services.AddSwaggerGen();

            _ = services.AddAuthentication(configureOptions: opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions: opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                };
            });

            _ = services.AddAuthorization();

            _ = services.AddCors(setupAction: opts => opts.AddPolicy(name: "CorsPolicy", configurePolicy: builder => builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

            return builder;
        }

        private static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            // repositories
            _ = services.AddScoped<IAccountRepository, AccountRepository>();

            // services
            _ = services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}