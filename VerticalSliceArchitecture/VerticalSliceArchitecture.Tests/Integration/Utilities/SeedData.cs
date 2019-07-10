using System;
using System.Collections.Generic;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Services;

namespace VerticalSliceArchitecture.Tests.Integration.Utilities
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDb appDb)
        {
            string password = "Test123$";
            var hasher = new Hasher();
            var salt = hasher.GetSalt();
            var passwordHash = hasher.GetHash(password, salt);

            var users = new List<User>()
            {
                new User("user123@gmail.com", "user123", UserRole.StandardUser, passwordHash, salt),
                 new User("user456@gmail.com", "user456", UserRole.StandardUser, passwordHash, salt),
                  new User("admin789@gmail.com", "admin789", UserRole.Admin, passwordHash, salt)
            };

            var games = new List<Game>()
            {
                new Game(Guid.NewGuid(),"Final Fantasy",DateTime.Now),
                 new Game(Guid.NewGuid(),"Metal Gear Solid",DateTime.Now),
                  new Game(Guid.NewGuid(),"Metal Slug",DateTime.Now),

            };

            appDb.Users.AddRange(users);
            appDb.Games.AddRange(games);
            appDb.SaveChanges();
        }
       
    }

   
}
        

