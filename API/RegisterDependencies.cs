using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace API
{
    public static class RegisterDependencies
    {
        public static void RegisterDbContext(IServiceCollection services ,  IConfiguration configuration) {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void RegisterRepositories(IServiceCollection services , IConfiguration configuration)
        {
            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<ClientRepository>();
        }
    }
}
