using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.BusinessLogic.Exceptions;
using Store.Data;
using Store.Data.Entities;
using Store.Infrastructure.HashProviders;
using Store.Infrastructure.Loggers;
using System.Collections.Generic;
using System.Collections;

namespace Store.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly StoreDbContext _dataContext;
        private readonly IPasswordHashProvider _hashProvider;
        private readonly ILogger _logger;

        private AccountService()
        {
            using var dbContext = new PostgresStoreDbContext();
        }

        public AccountService(StoreDbContext dataContext, IPasswordHashProvider hashProvider, ILogger logger) : this()
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
            {
                _logger.Log("User cannot be null");
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                // HACK: нужно ли проверять, имеются ли в бд юзены с одинаковыми id или login и как на реагировать?
                user = _dataContext.Users.SingleOrDefault(x => x.Id == user.Id);

                if (user is null) _logger.Log("User not found");

                if (user.Disabled) throw new DisableException("User is already disabled");

                user.Disabled = true;

                AccountHistory accountHistoryDisabled = CreateAccountHistory(EventType.Disabled, user, null);
                _dataContext.AccountHistories.Add(accountHistoryDisabled);
                _dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                return;
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
                {
                    _logger.Log("User not found");
                    return null;
                }

                if (user.Disabled) throw new LoginException("User was deleted");

                if (!_hashProvider.Verify(password, user.Password)) throw new LoginException("Wrong password");

                AccountHistory accountHistorySuccessfullLogin = CreateAccountHistory(EventType.SuccessfullLogin, user, null);

                _dataContext.AccountHistories.Add(accountHistorySuccessfullLogin);
                _dataContext.SaveChanges();
            }
            catch (LoginException ex)
            {
                // (done2 fixed) тут юзер может быть null, если упало до получения юзера из БД
                //user.AccountHistory.Add(new AccountHistory
                //{
                //    DateTimeOffset = DateTimeOffset.UtcNow,
                //    EventType = EventType.LoginAttempt,
                //    //User = user,
                //    UserId = user.Id,
                //    ErrorMessage = ex.Message
                //});

                AccountHistory accountHistoryLoginAttempt = CreateAccountHistory(EventType.LoginAttempt, user, ex.Message);

                _dataContext.AccountHistories.Add(accountHistoryLoginAttempt);
                _dataContext.SaveChanges();

                _logger.Log($"[{ex}] {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                return null;
            }

            return user;
        }

        public void LogOut(User user)
        {
            if (user is null)
            {
                _logger.Log("User cannot be null");
                return;
            }

            try
            {

                user = _dataContext.Users.SingleOrDefault(x => x.Id == user.Id);

                if (user is null) throw new LogoutException("User not found");

                if (!user.Disabled) throw new LoginException("User is not exist");

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
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
            }
        }

        public void SignUp(string login, string password, string passwordConfirmation)
        {
            // TODO: (done)

            // проверить логин на уникальность
            // проверить совпадение паролей
            // проверить требования к паролю (В инфраструктуру - интерфейс IPasswordValidator) (план-максимум: посмотри, что такое nuget FluentValidation)

            // не забыть выставить CreatedOn и IsActive

            

            try
            {
                // Validating password... Comming soon :)
                // if (password.IsBad())
                // {
                //    _logger.Log("Password does not meet requirements");
                //    return;
                // }
                // Password - ok

                if (password != passwordConfirmation)
                {
                    _logger.Log("Passwords don't math");
                    throw new SignupException("Passwords don't math");
                }

                string hashedPassword = _hashProvider.GenerateHash(password);

                var existingUser = _dataContext.Users.SingleOrDefault(x => x.Login == login);
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
