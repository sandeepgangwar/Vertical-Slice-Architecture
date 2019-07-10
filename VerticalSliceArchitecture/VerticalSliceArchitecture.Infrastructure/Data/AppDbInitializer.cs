using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Services;

namespace VerticalSliceArchitecture.Infrastructure.Data
{
    public class AppDbInitializer
    {
        private readonly AppDb _db;

        public AppDbInitializer(AppDb db)
        {
            _db = db;
        }

        public async Task Seed()
        {
            await _db.Database.MigrateAsync();

            if (!_db.Users.Any())
            {
                await CreateUsers();
            }
            if (!_db.Games.Any())
            {
                await CreateGames();
            }

        }

        private async Task CreateUsers()
        {
            for (int i = 0; i < 10; i++)
            {
                string password = "Test123$";
                var hasher = new Hasher();
                var salt = hasher.GetSalt();
                var passwordHash = hasher.GetHash(password, salt);

                var user = new User($"user{i + 1}@gmail.com", $"user{i + 1}", UserRole.StandardUser, passwordHash, salt);

                await _db.AddAsync(user);
                await _db.SaveChangesAsync();
            }
        }
        private async Task CreateGames()
        {
            var users = _db.Users.ToList();

            foreach (var user in users)
            {             

                for (int i = 0; i < users.Count; i++)
                {
                    var game = new Game(users[i].Id, $"title {i + 1}", DateTime.Parse("2000-02-02"));

                    await _db.Games.AddAsync(game);
                    await _db.SaveChangesAsync();
                }

            }
        }
    }
}
