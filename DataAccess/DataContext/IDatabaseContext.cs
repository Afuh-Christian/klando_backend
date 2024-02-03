using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public interface IDatabaseContext
    {
        DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors|DynamicallyAccessedMemberTypes.NonPublicConstructors|DynamicallyAccessedMemberTypes.PublicFields|DynamicallyAccessedMemberTypes.NonPublicFields|DynamicallyAccessedMemberTypes.PublicProperties|DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>() where TEntity : class;
        DatabaseFacade DatabaseFacade { get; }



        DbSet<Trip> Trips { get; }
        DbSet<Client> Clients { get; }
        DbSet<ConfirmedRide> ConfirmedRides { get;}
        DbSet<Image> Images { get; }
        DbSet<Driver> Drivers { get; }
        DbSet<Location> Locations { get; }  
        DbSet<Payment> Payments { get; }
        DbSet<Transaction> Transactions { get; }
        DbSet<Order> Orders { get; } 





    }
}

