using IamApi.Domain;
using System;
using System.Collections.Generic;

namespace IamApi.Repositories
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);

        List<User> GetAll();

        User GetById(Guid id);

        User Save(User user);
    }
}