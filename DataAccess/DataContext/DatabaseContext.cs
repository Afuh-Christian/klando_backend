using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DatabaseFacade DatabaseFacade => throw new NotImplementedException();

        public DbSet<Trip> Trips => throw new NotImplementedException();

        public DbSet<Client> Clients => throw new NotImplementedException();

        public DbSet<ConfirmedRide> ConfirmedRides => throw new NotImplementedException();

        public DbSet<Image> Images => throw new NotImplementedException();

        public DbSet<Driver> Drivers => throw new NotImplementedException();

        public DbSet<Location> Locations => throw new NotImplementedException();

        public DbSet<Payment> Payments => throw new NotImplementedException();

        public DbSet<Transaction> Transactions => throw new NotImplementedException();

        public DbSet<Order> Orders => throw new NotImplementedException();
    }
}
