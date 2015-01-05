using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public interface IUser
    {
        int UserId { get; set; }
        string Identify();
    }

    public class User : IUser
    {
        public int UserId { get; set; }

        //Need empty ctor at the moment so the registry resolves it correctly...FIX THIS
        public User() {}

        public string Identify()
        {
            return string.Format("I'm user {0}!", UserId);
        }
    }
}
