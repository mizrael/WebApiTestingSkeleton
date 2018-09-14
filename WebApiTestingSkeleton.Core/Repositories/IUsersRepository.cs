using System;
using System.Collections.Generic;
using WebApiTestingSkeleton.Core.Models;

namespace WebApiTestingSkeleton.Core.Repositories
{
    public interface IUsersRepository
    {
        IReadOnlyCollection<User> GetAll();
        User GetById(Guid id);
        bool Remove(Guid id);
        User Upsert(User user);
    }
}