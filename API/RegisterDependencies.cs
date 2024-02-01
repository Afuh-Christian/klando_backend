using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace API
{
    public static class RegisterDependencies
    {
        // Add DbContext
        public static void RegisterDbContext(IServiceCollection services ,  IConfiguration configuration) {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        

        // Add Repositories
        public static void RegisterRepositories(IServiceCollection services , IConfiguration configuration)
        {
            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<ClientRepository>();
        }

        // Add Cors
        public static void RegisterCORS(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("klando_frontend", builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
        }
    }
}
