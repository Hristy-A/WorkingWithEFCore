using System;
using System.Linq;
using Store.BusinessLogic.Services;
using Store.Infrastructure.HashProviders;
using Store.Data.Entities;

namespace Sandbox
{
    class Program
    {
        //TODO:
        // Сделать таблицу  AccountHistory : [PK] int Id, int UserId?, string UserLogin?, EventType EventType, DateTimeOffset Timestamp, string ErrorMessage?
        // EventType {SuccessfullLogin, SuccessfullLogout, LoginAttempt, Registration}
       

        static void Main(string[] args)
        {
            IPasswordHashProvider hashProvider = new BCryptHashProvider();

            IAccountService accountService = new AccountService(hashProvider);

            Console.Write("Enter login: ");
            var login = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var user = accountService.Login(login, password);

            if (user != null)
            {
                Console.WriteLine($"User '{user.Login}' is in roles: {string.Join(", ", user.Roles.Select(x => x.ShortName))}");
            }
            else
            {
                Console.WriteLine("Smth goes wrong");
            }

            Console.ReadLine();
        }
    }
}
