using IamApp.Domain;
using System;

namespace IamApp.Repositories
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);

        User GetById(Guid id);

        User Save(User user);
    }
}