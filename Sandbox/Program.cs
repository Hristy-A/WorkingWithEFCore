using System;
using System.Linq;
using Store.BusinessLogic.Services;
using Store.Infrastructure.HashProviders;
using Store.Data.Entities;
using Store.Infrastructure.Loggers;

namespace Sandbox
{
    class Program
    {
        //TODO: (done)
        // Сделать таблицу  AccountHistory : [PK] int Id, int UserId?, string UserLogin?, EventType EventType, DateTimeOffset Timestamp, string ErrorMessage?
        // EventType {SuccessfullLogin, SuccessfullLogout, LoginAttempt, Registration}
       

        static void Main()
        {
            IPasswordHashProvider hashProvider = new BCryptHashProvider();
            ILogger logger = new DebugWindowLogger();

            IAccountService accountService = new AccountService(hashProvider, logger);

            while (true)
            {
                Console.WriteLine("Select option:\n" +
                    "    1 - SignUp,\n" +
                    "    2 - LogIn\n");
                
                string option = Console.ReadLine().ToLower();

                switch (option)
                {
                    case "1" or "signup":
                        Signup(accountService);
                        break;

                    case "2" or "login":
                        Login(accountService);
                        break;
                }
            }
        }

        private static void Signup(IAccountService accountService)
        {
            Console.Write("Enter login: ");
            var login = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            Console.Write("Reenter password: ");
            var againPasswrod = Console.ReadLine();

            accountService.SignUp(login, password, againPasswrod);
        }

        public static void Login(IAccountService accountService)
        {
            Console.Write("Enter login: ");
            var login = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var user = accountService.LogIn(login, password);
        }


    }
}
