using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Core.Domain.Identity;

namespace VerticalSliceArchitecture.Infrastructure.Data
{

    public static class SessionConfiguration
    {
        public static void Setup(ModelBuilder builder)
        {
            builder.Entity<Session>()
                .HasKey(s => new { s.UserId, s.DeviceId });

            builder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions);

            builder.Entity<Session>()
                .Property(s => s.DeviceId)
                .HasMaxLength(64);

            builder.Entity<Session>()
                .Property(s => s.DeviceName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Entity<Session>()
                .Property(s => s.RefreshToken)
                .IsRequired()
                .HasMaxLength(64);
        }
    }


}