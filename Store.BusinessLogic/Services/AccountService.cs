using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.BusinessLogic.Exceptions;
using Store.Data;
using Store.Data.Entities;
using Store.Infrastructure.HashProviders;

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

        public void Disable(int userId)
        {
            try
            {
                var user = _dataContext.Users.SingleOrDefault(x => x.Id == userId);

                if (user is null)
                    throw new DisableException("User not found");

                if (user.Disabled)
                    throw new DisableException("User is already disabled");

                user.Disabled = true;

                var entry = CreateAccountHistoryEntry(EventType.Disabled, user, null);
                _dataContext.AccountHistory.Add(entry);
                _dataContext.SaveChanges();
            }
            catch (DisableException ex)
            {
                _logger.LogError(ex, ex.Message);
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

                var entry = CreateAccountHistoryEntry(EventType.SuccessfullLogin, user, null);

                _dataContext.AccountHistory.Add(entry);
                _dataContext.SaveChanges();
            }
            catch (LoginException ex)
            {
                var entry = CreateAccountHistoryEntry(EventType.LoginAttempt, user, ex.Message);

                _dataContext.AccountHistory.Add(entry);
                _dataContext.SaveChanges();

                _logger.LogError(ex, ex.Message);
                throw;
            }

            return user;
        }

        public void LogOut(int userId)
        {
            User user = null;

            try
            {
                user = _dataContext.Users.SingleOrDefault(x => x.Id == userId);

                if (user is null)
                    throw new LogoutException("User not found");

                if (!user.Disabled)
                    throw new LogoutException("User is not exist");

                var entry = CreateAccountHistoryEntry(EventType.SuccessfullLogout, user, null);

                _dataContext.AccountHistory.Add(entry);
                _dataContext.SaveChanges();
            }
            catch (LogoutException ex)
            {
                var entry = CreateAccountHistoryEntry(EventType.LogoutAttempt, user, ex.Message);

                _dataContext.AccountHistory.Add(entry);
                _dataContext.SaveChanges();

                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void SignUp(string login, string password, string passwordConfirmation)
        {
            try
            {
                if (password != passwordConfirmation)
                    throw new SignupException("Passwords don't math");

                #region Validation Password
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
                #endregion

                string hashedPassword = _hashProvider.GenerateHash(password);

                User existingUser = _dataContext.Users.SingleOrDefault(x => x.Login == login);

                if (existingUser is null)
                {
                    var user = new User
                    {
                        Login = login,
                        Password = hashedPassword,
                        CreatedOn = DateTimeOffset.UtcNow,
                        Disabled = false
                    };

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
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private static AccountHistoryEntry CreateAccountHistoryEntry(EventType eventType, User user, string errorMessage) =>
            new AccountHistoryEntry
            {
                EventType = eventType,
                User = user,
                ErrorMessage = errorMessage,
                DateTimeOffset = DateTimeOffset.UtcNow
            };
    }
}
