using System;

namespace WebApiTestingSkeleton.Core.Models
{
    public class User
    {
        public User(Guid id, string fullname, string email)
        {
            Id = id;
            Email = email;
            Fullname = fullname;
        }

        public Guid Id { get; }
        public string Fullname { get; }
        public string Email { get; }

        public static readonly User NullUser = new User(Guid.Empty, "[not set]", "[not set]");
    }
}