using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfig
{
    public class ConfirmRideEntityConfig : IEntityTypeConfiguration<ConfirmedRide>
    {
        public void Configure(EntityTypeBuilder<ConfirmedRide> builder)
        {
            builder.Property(e => e.ConfirmedRideId).HasDefaultValueSql("(newid())");
            builder.HasKey(e => e.ConfirmedRideId);
        }

        
    }
}
