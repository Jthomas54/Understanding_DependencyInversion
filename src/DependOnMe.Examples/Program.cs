using System;

using DependOnMe;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var reg = new Registry();

            //Register the types so it knows how to resolve the concrete types
            reg.Register<IUser, User>();
            reg.Register<IUserRepo, InMemoryUserRepo>();

            var userRepo = reg.GetInstance<IUserRepo>();

            Console.WriteLine(userRepo.Identify());

            foreach (var user in userRepo.GetAllUsers())
            {
                Console.WriteLine(user.Identify());
            }
        }
    }
}
