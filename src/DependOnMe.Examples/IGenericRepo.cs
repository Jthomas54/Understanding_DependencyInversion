using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    public interface IGenericRepo<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        string Identify();
    }

    public class GenericUserRepo : IGenericRepo<IUser>
    {
        private readonly List<IUser> _Users;

        public UserRepo(IUser user)
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

        public IEnumerable<IUser> GetAll()
        {
            return _Users;
        }


        public string Identify()
        {
            return string.Format("I'm a implementation of a generic User Repo!  I have {0} users stored.", _Users.Count);
        }
    }
}
