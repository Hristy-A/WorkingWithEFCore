using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.BusinessLogic.Exceptions;
using Store.Data;
using Store.Data.Entities;
using Store.Infrastructure.HashProviders;
using Store.Infrastructure.Loggers;

namespace Store.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly StoreDbContext _dataContext;
        private readonly IPasswordHashProvider _hashProvider;
        private readonly ILogger _logger;

        public AccountService(StoreDbContext dataContext, IPasswordHashProvider hashProvider, ILogger logger)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _hashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private AccountHistory CreateAccountHistory(EventType eventType, User user, string errorMessage) =>
            new AccountHistory
            {
                EventType = eventType,
                User = user,
                ErrorMessage = errorMessage,
                DateTimeOffset = DateTimeOffset.UtcNow
            };

        public void Disable(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            try
            {
                user = _dataContext.Users.SingleOrDefault(x => x.Id == user.Id);

                if (user is null)
                    throw new DisableException("User not found");

                if (user.Disabled) 
                    throw new DisableException("User is already disabled");

                user.Disabled = true;

                AccountHistory accountHistoryDisabled = CreateAccountHistory(EventType.Disabled, user, null);
                _dataContext.AccountHistories.Add(accountHistoryDisabled);
                _dataContext.SaveChanges();
            }
            catch (DisableException ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                throw;
            }
        }

        public User LogIn(string login, string password)
        {
            User user = null;

            try
            {
                user = _dataContext.Users
                    .Include(x => x.Roles)
                    .SingleOrDefault(x => x.Login == login);

                if (user is null)
                    throw new LoginException("User not found");

                if (user.Disabled) 
                    throw new LoginException("User was deleted");

                if (!_hashProvider.Verify(password, user.Password)) 
                    throw new LoginException("Wrong password");

                AccountHistory accountHistorySuccessfullLogin = CreateAccountHistory(EventType.SuccessfullLogin, user, null);

                _dataContext.AccountHistories.Add(accountHistorySuccessfullLogin);
                _dataContext.SaveChanges();
            }
            catch (LoginException ex)
            {
                AccountHistory accountHistoryLoginAttempt = CreateAccountHistory(EventType.LoginAttempt, user, ex.Message);

                _dataContext.AccountHistories.Add(accountHistoryLoginAttempt);
                _dataContext.SaveChanges();

                _logger.Log($"[{ex}] {ex.Message}");
                throw;
            }

            return user;
        }

        public void LogOut(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            try
            {
                user = _dataContext.Users.SingleOrDefault(x => x.Id == user.Id);

                if (user is null) 
                    throw new LogoutException("User not found");

                if (!user.Disabled) 
                    throw new LogoutException("User is not exist");

                AccountHistory accountHistorySuccessfullLogout = CreateAccountHistory(EventType.SuccessfullLogout, user, null);

                _dataContext.AccountHistories.Add(accountHistorySuccessfullLogout);
                _dataContext.SaveChanges();
            }
            catch (LogoutException ex)
            {
                AccountHistory accountHistorySuccessfullLogout = CreateAccountHistory(EventType.LogoutAttempt, user, ex.Message);

                _dataContext.AccountHistories.Add(accountHistorySuccessfullLogout);
                _dataContext.SaveChanges();

                _logger.Log($"[{ex}] {ex.Message}");
                throw;
            }
        }

        public void SignUp(string login, string password, string passwordConfirmation)
        {
            try
            {
                if (password != passwordConfirmation)
                    throw new SignupException("Passwords don't math");

                // TODO:

                // проверить логин на уникальность
                // проверить совпадение паролей
                // проверить требования к паролю (В инфраструктуру - интерфейс IPasswordValidator) (план-максимум: посмотри, что такое nuget FluentValidation)

                // Validating password... Comming soon :)
                // if (password.IsBad())
                // {
                //    _logger.Log("Password does not meet requirements");
                //    return;
                // }
                // Password - ok/not ok

                string hashedPassword = _hashProvider.GenerateHash(password);

                User existingUser = _dataContext.Users.SingleOrDefault(x => x.Login == login);

                if (existingUser is null)
                {
                    User user = CreateUser(login, hashedPassword);
                    _dataContext.Users.Add(user);
                    _dataContext.SaveChanges();
                }
                else
                {
                    throw new SignupException("This login already exists");
                }
            }
            catch (SignupException ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                throw;
            }

            static User CreateUser(string login, string hashedPassword) =>
                new User
                {
                    Login = login,
                    Password = hashedPassword,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Disabled = false
                };
        }
    }
}
