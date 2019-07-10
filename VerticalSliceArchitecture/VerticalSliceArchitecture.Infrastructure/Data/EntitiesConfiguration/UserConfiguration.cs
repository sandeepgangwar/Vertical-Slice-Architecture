using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Core.Domain.Identity;

namespace VerticalSliceArchitecture.Infrastructure.Data
{
    
    public static class UserConfiguration
    {
        public static void Setup(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasKey(u => u.Id);

            builder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50);

            builder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Entity<User>()
                .Property(u => u.PasswordSalt)
                .IsRequired()
                .HasMaxLength(64);

            builder.Entity<User>()
                .HasMany(u => u.Sessions)
                .WithOne(s => s.User);

            builder.Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(s => s.User);

            var sessionsNavigation = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.Sessions));
            sessionsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

}