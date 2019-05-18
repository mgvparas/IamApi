using IamApi.Domain;
using IamApi.Extensions;
using IamApi.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace IamApi.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public List<User> GetAll() => _context.Users.ToList();

        public User GetById(Guid id) => _context.Users.Find(id);

        public User Save(User user)
        {
            if (_context.Users.Any(x => x.Username == user.Username)) throw new Exception("Username is already taken.");

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Authenticate(string username, string password)
        {
            if (username.IsNullOrWhiteSpace()) throw new Exception("Username is required.");
            if (password.IsNullOrWhiteSpace()) throw new Exception("Password is required.");

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
            if (password.IsNullOrWhiteSpace()) throw new Exception("Password should contain at least one(1) character.");
            if (storedHash.Length != 64) throw new Exception("Password hash should be 64 bytes.");
            if (storedSalt.Length != 128) throw new Exception( "Password salt should be 128 bytes.");

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