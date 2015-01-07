using System;

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

        public string Identify()
        {
            return string.Format("I'm user {0}!", UserId);
        }
    }
}
