using System;

using DependOnMe;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var reg = new Registry();


            Console.WriteLine("Interface -> Concrete Resolution:");
            Console.WriteLine("------------------------------------------------");
            //Register the types so it knows how to resolve the concrete types
            reg.Register<IUser, User>();
            reg.Register<IUserRepo, InMemoryUserRepo>();

            var userRepo = reg.GetInstance<IUserRepo>();

            Console.WriteLine(userRepo.Identify());

            foreach (var user in userRepo.GetAllUsers())
            {
                Console.WriteLine(user.Identify());
            }


            Console.WriteLine("Closed Generic Resolution:");
            Console.WriteLine("------------------------------------------------");
            reg.Register<IGenericRepo<IUser>, GenericUserRepo>();

            var userRepoWithClosedGenerics = reg.GetInstance<IGenericRepo<IUser>>();

            Console.WriteLine(userRepoWithClosedGenerics.Identify());

            foreach (var user in userRepoWithClosedGenerics.GetAll())
            {
                Console.WriteLine(user.Identify());
            }
        }
    }
}
