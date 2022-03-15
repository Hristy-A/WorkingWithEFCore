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
    public class AccountService : IAccountService, IEnumerable<User>
    {
        private readonly IPasswordHashProvider _hashProvider;
        private readonly ILogger _logger;

        private readonly List<User> _usersOnline;

        private AccountService()
        {
            using var dbContext = new StoreDbContext();
            _usersOnline = new List<User>(dbContext.Users.Count(x => !x.Disabled));
        }

        public AccountService(IPasswordHashProvider hashProvider, ILogger logger) : this()
        {
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
                using (var dbContext = new StoreDbContext())
                {
                    // HACK: нужно ли проверять, имеются ли в бд юзены с одинаковыми id или login и как на реагировать?
                    user = dbContext.Users.SingleOrDefault(x => x.Id == user.Id);

                    if (user is null) _logger.Log("User not found");

                    if (user.Disabled) throw new DisableException("User is already disabled");

                    if (_usersOnline.Contains(user))
                    {
                        LogOut(user);
                        _usersOnline.Remove(user);
                    }

                    AccountHistory accountHistoryDisabled = CreateAccountHistory(EventType.Disabled, user, null);

                    dbContext.AccountHistories.Add(accountHistoryDisabled);
                    user.Disabled = true;

                    dbContext.SaveChanges();
                }
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
                using (var dbContext = new StoreDbContext())
                {
                    user = dbContext.Users
                        .Include(x => x.Roles)
                        .SingleOrDefault(x => x.Login == login);

                    if (user is null)
                    {
                        _logger.Log("User not found");
                        return null;
                    }

                    if (user.Disabled) throw new LoginException("User was deleted");

                    if (!_hashProvider.Verify(password, user.Password)) throw new LoginException("Wrong password");

                    if (_usersOnline.Contains(user)) throw new LoginException("User already online");

                    AccountHistory accountHistorySuccessfullLogin = CreateAccountHistory(EventType.SuccessfullLogin, user, null);

                    dbContext.AccountHistories.Add(accountHistorySuccessfullLogin);
                    dbContext.SaveChanges();
                }

                _usersOnline.Add(user);
            }
            catch (LoginException ex)
            {
                using var dbContext = new StoreDbContext();
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

                dbContext.AccountHistories.Add(accountHistoryLoginAttempt);
                dbContext.SaveChanges();

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
                using (var dbContext = new StoreDbContext())
                {
                    user = dbContext.Users.SingleOrDefault(x => x.Id == user.Id);

                    if (user is null) throw new LogoutException("User not found");

                    if (!user.Disabled) throw new LoginException("User is not exist");

                    if (!_usersOnline.Contains(user)) 
                        throw new InvalidOperationException("User cannot LogOut from offline state");

                    AccountHistory accountHistorySuccessfullLogout = CreateAccountHistory(EventType.SuccessfullLogout, user, null);

                    dbContext.AccountHistories.Add(accountHistorySuccessfullLogout);
                    dbContext.SaveChanges();
                }
                _usersOnline.Remove(user);
            }
            catch (LogoutException ex)
            {
                using var dbContext = new StoreDbContext();

                AccountHistory accountHistorySuccessfullLogout = CreateAccountHistory(EventType.LogoutAttempt, user, ex.Message);

                dbContext.AccountHistories.Add(accountHistorySuccessfullLogout);
                dbContext.SaveChanges();

                _logger.Log($"[{ex}] {ex.Message}");
            }
            catch(InvalidOperationException ex)
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

            if (password != passwordConfirmation)
            {
                _logger.Log("Passwords don't math");
                return;
            }

            try
            {
                // Validating password... Comming soon :)
                // if (password.IsBad())
                // {
                //    _logger.Log("Password does not meet requirements");
                //    return;
                // }
                // Password - ok

                string hashedPassword = _hashProvider.GenerateHash(password);

                using (StoreDbContext dbContext = new StoreDbContext())
                {
                    var existingUser = dbContext.Users.SingleOrDefault(x => x.Login == login);
                    if (existingUser is null)
                    {
                        User user = CreateUser(login, hashedPassword);
                        dbContext.Users.Add(user);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        throw new SignupException("This login already exists");
                    }
                }
            }
            catch (SignupException ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                return;
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

        public IEnumerator<User> GetEnumerator()
        {
            return _usersOnline.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
