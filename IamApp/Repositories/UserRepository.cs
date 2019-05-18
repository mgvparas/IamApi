using System;
using System.Collections.Generic;
using IamApp.Domain;

namespace IamApp.Repositories
{
    public class UserRepository
    {
        private List<User> _users = new List<User>();

        public void Save(User user) => _users.Add(user);
    }
}