using System;
using Store.BusinessLogic.Services;
using Store.Data;
using Store.Infrastructure.HashProviders;
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
            #region Build Options
//#if (DEBUG && testing)
//            {
//                // heating database
//                using (Store.Data.StoreDbContext dbContext = new Store.Data.StoreDbContext()) dbContext.EventTypeInfo.FirstOrDefault();
//            }
//#endif
            #endregion

            IPasswordHashProvider hashProvider = new BCryptHashProvider();
            ILogger logger = new DebugWindowLogger();
            StoreDbContext dbContext = new PostgresStoreDbContext();

            IAccountService accountService = new AccountService(dbContext, hashProvider, logger);

            while (true)
            {
                Console.Write(@"" +
                    "Select option:\n" +
                    "    1 - SignUp,\n" +
                    "    2 - LogIn\n" +
                    "    3 - LogOut\n" +
                    "    4 - Disable\n" +
                    "    5 - ShowUsersOnline\n" +
                    "    6 - LogIn default (login 3 hardcoded users)\n" +
                    "    7 - SignUp default (signup 5 default users)\n>");

                string option = Console.ReadLine().ToLower();

                switch (option)
                {
                    case "1" or "signup":
                        SignUp(accountService);
                        break;
                    case "2" or "login":
                        LogIn(accountService);
                        break;
                    case "3" or "logout":
                        LogOut(accountService);
                        break;
                    case "4" or "disable":
                        Disable(accountService);
                        break;
                    case "5" or "show" or "showusers":
                        ShowOnlineUsers(accountService);
                        break;
                    case "6" or "def":
                        LogInDefault(accountService);
                        break;
                    case "7" or "init":
                        SignUpDefault(accountService);
                        break;
                    default:
                        Console.WriteLine("Operation not exists");
                        break;
                }

                
            }
        }

        private static void SignUpDefault(IAccountService accountService)
        {
            accountService.SignUp("subject", "12345678", "12345678");
            accountService.SignUp("Klubnica", "password", "password");
            accountService.SignUp("Fibonach", "passgood213", "passgood213");
            accountService.SignUp("Someuser", "123123123", "123123123");
            accountService.SignUp("Anonimus123", "jfe14k124", "jfe14k124");
            accountService.SignUp("Pingvin", "fwrwewgwbwcw", "fwrwewgwbwcw");
        }

        private static void LogInDefault(IAccountService accountService)
        {
            accountService.LogIn("subject", "12345678");
            accountService.LogIn("Klubnica", "password");
            accountService.LogIn("Someuser", "123123123");
            accountService.LogIn("Anonimus123", "jfe14k124");
        }

        private static void ShowOnlineUsers(IAccountService accountService)
        {
            Console.WriteLine("Not implemented!");
        }

        private static void Disable(IAccountService accountService)
        {
            Console.Write("Not implemented!");
        }

        private static void LogOut(IAccountService accountService)
        {
            Console.Write("Not implemented!");
        }

        private static void SignUp(IAccountService accountService)
        {
            Console.Write("Enter login: ");
            var login = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            Console.Write("Confirm password: ");
            var againPasswrod = Console.ReadLine();

            accountService.SignUp(login, password, againPasswrod);
        }

        public static void LogIn(IAccountService accountService)
        {
            Console.Write("Enter login: ");
            var login = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            accountService.LogIn(login, password);
        }
    }
}
