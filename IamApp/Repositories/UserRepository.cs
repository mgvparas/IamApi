using IamApp.Domain;
using IamApp.Extensions;
using IamApp.Utils;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace IamApp.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User GetById(Guid id) => _context.Users.Find(id);

        public User Save(User user)
        {
            Contract.Requires(!_context.Users.Any(x => x.Username == user.Username), "Username is already taken.");

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user == null) return null;

            return PasswordHashIsVerified(
                password,
                user.PasswordHash,
                user.PasswordSalt
            )
                ? user
                : null;
        }

        private static bool PasswordHashIsVerified(string password, byte[] storedHash, byte[] storedSalt)
        {
            Contract.Requires(!password.IsNullOrWhiteSpace(), "Password should contain at least one(1) character.");
            Contract.Requires(storedHash.Length == 64, "Password hash should be 64 bytes.");
            Contract.Requires(storedSalt.Length == 128, "Password salt should be 128 bytes.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}