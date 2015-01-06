using System;
using System.Collections.Generic;

namespace Examples
{
    interface IUserRepo
    {
        IEnumerable<IUser> GetAllUsers();
        string Identify();
    }

    public class InMemoryUserRepo : IUserRepo
    {
        private readonly List<IUser> _Users;

        //Testing the injection of another type to make sure it resolves sub-dependencies
        //Yes, this is a horrible example of DI :P
        public InMemoryUserRepo(IUser user)
        {
            _Users = new List<IUser>()
                {
                    new User{UserId = 1},
                    new User{UserId = 2},
                    new User{UserId = 3},
                    new User{UserId = 4},
                };

            user.UserId = 999;

            _Users.Add(user);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _Users;
        }

        public string Identify()
        {
            return string.Format("I'm a In-Memory User Repo!  I have {0} users stored.", _Users.Count);
        }
    }
}
