using System;

using DependOnMe;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //Only works with public constructors as of now and an empty ctor is needed to find the overloaded one
            var reg = new Registry();

            //Registry the types so it knows how to resolve one type to another
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
