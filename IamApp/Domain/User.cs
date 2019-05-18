using IamApp.Extensions;
using System;
using System.Diagnostics.Contracts;

namespace IamApp.Domain
{
    public class User
    {
        public User(string username, string password, string email)
        {
            if (username.IsNullOrWhiteSpace()) throw new Exception("Username is required.");
            if (password.IsNullOrWhiteSpace()) throw new Exception("Password is required.");
            if (email.IsNullOrWhiteSpace()) throw new Exception("Email is required.");

            Username = username;
            Password = password;
            Email = email;

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public Guid Id { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public byte[] PasswordHash { get; internal set; }

        public byte[] PasswordSalt { get; internal set; }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password.IsNullOrWhiteSpace()) throw new Exception("Password is required.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}