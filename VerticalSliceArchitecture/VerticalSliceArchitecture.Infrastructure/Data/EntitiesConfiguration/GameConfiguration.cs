using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VerticalSliceArchitecture.Core.Domain.Games;

namespace VerticalSliceArchitecture.Infrastructure.Data.EntitiesConfiguration
{
    public class GameConfiguration
    {
        public static void Setup(ModelBuilder builder)
        {
            builder.Entity<Game>()
                .HasKey(g => g.Id);
          
            builder.Entity<Game>()
                .Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(255);
          
            builder.Entity<Game>()
                .HasOne(g => g.User)
                .WithMany(g => g.Games);

        }
    }
}
