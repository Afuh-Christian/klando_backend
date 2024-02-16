using DataAccess.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository.Repositories;
using System;
using System.Text;

namespace API;

public static class RegisterDependencies
{


    // Add DbContext
    public static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }


    // Add Repositories
    public static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<IDatabaseContext, DatabaseContext>();
        services.AddTransient<ClientRepository>();
        services.AddTransient<DriverRepository>();
        services.AddTransient<TripRepository>();
        services.AddTransient<PaymentRepository>();
        services.AddTransient<TransactionRepository>();
        services.AddTransient<OrderRepository>();
        services.AddTransient<ConfirmedRideRepository>();
        services.AddTransient<ImageRepository>();
        services.AddTransient<LocationRepository>();
    }


    // Add Cors
    public static void RegisterCORS(IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("klando_frontend", builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
    }


    // Authentication . 
    public static void RegisterAuth(IServiceCollection services, IConfiguration configuration)
    {


        //Add Identity & JWT authentication
        //Identity
        services//.AddIdentityApiEndpoints<IdentityUser>()
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddSignInManager()
           // .AddUserManager<IdentityUser>()
           // .AddUserStore<DatabaseContext>()
            .AddRoles<IdentityRole>();


        // JWT 
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

    }
}
