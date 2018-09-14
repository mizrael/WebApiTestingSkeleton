using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using WebApiTestingSkeleton.Core.Models;

namespace WebApiTestingSkeleton.Core.Repositories
{
    public class InMemoryUsersRepository : IUsersRepository
    {
        private static readonly ConcurrentDictionary<Guid, User> _items;

        static InMemoryUsersRepository()
        {
            _items = new ConcurrentDictionary<Guid, User>();
        }

        public IReadOnlyCollection<User> GetAll()
        {
            return ImmutableArray.CreateRange(_items.Values);
        }

        public User GetById(Guid id)
        {
            return _items.TryGetValue(id, out User result) ? result : User.NullUser;
        }

        public User Upsert(User user)
        {
            if(null == user)
                throw new ArgumentNullException(nameof(user));
            return _items.AddOrUpdate(user.Id, user, (id, u) => user);
        }

        public bool Remove(Guid id)
        {
            return _items.TryRemove(id, out User _);
        }
        
    }
}
