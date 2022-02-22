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
        private readonly IPasswordHashProvider _hashProvider;
        private readonly ILogger _logger;

        public AccountService(IPasswordHashProvider hashProvider, ILogger logger)
        {
            _hashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Disable(User user)
        {
            // TODO: (done)

            // проверить, есть ли такой пользователь вообще
            // проверить, не деактивирован ли он
            try
            {
                if (user is null) // TODO: до трая бросить ошибку
                {
                    _logger.Log("User cannot be null");
                    return;
                }

                using (StoreDbContext dbContext = new StoreDbContext())
                {
                    user = dbContext.Users.SingleOrDefault(x => x.Id == user.Id);

                    if(user is null)
                    {
                        _logger.Log("User not found");
                    }   

                    if (!user.IsActive) throw new DisableException("User is not exist");

                    user.AccountHistory.Add(new AccountHistory
                    {
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        EventType = EventType.Disabled,
                        //User = user,
                        UserId = user.Id
                    });
                    user.IsActive = false;

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
                }

                if(user is null)
                {
                    _logger.Log("User not found");
                    return null;
                }

                if (!user.IsActive)
                {
                    throw new LogInException("User is not active");
                }

                if (!_hashProvider.Verify(password, user.Password))
                {
                    throw new LogInException("Wrong password");
                }

                // TODO: обновлять AccountHistory не через юзера, а напрямую через ДБсет -> не придется трекать юзера сквозь контексты
                using (var dbContext = new StoreDbContext())
                {
                    user.AccountHistory.Add(new AccountHistory
                    {
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        EventType = EventType.SuccessfullLogIn,
                        //User = user,
                        UserId = user.Id
                    });

                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                using (StoreDbContext dbContext = new StoreDbContext())
                {
                    //тут юзер может быть null, если упало до получения юзера из БД
                    user.AccountHistory.Add(new AccountHistory
                    {
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        EventType = EventType.LogInAttempt,
                        //User = user,
                        UserId = user.Id,
                        ErrorMessage = ex.Message
                    });

                    dbContext.Update(user);
                    dbContext.SaveChanges();
                }

                _logger.Log($"[{ex}] {ex.Message}");
                return null;
            }

            return user;
        }

        public void LogOut(User user)
        {
            // TODO: реализовать запись в таблицу истории действий, когда она будет добавлена
            try
            {
                if (user is null) //TODO тут бы ошибку бросить - вне try
                {
                    _logger.Log("User cannot be null");
                    return;
                }

                using (StoreDbContext dbContext = new StoreDbContext())
                {
                    user = dbContext.Users.SingleOrDefault(x => x.Id == user.Id);

                    if (user is null) //TODO тянет на исключение
                    {
                        _logger.Log("User not found");
                        return;
                    }

                    if (!user.IsActive) // TODO не похоже на ошибку, но можно залогировать странное поведение
                    {
                        throw new LogInException("User is not exist");
                    }
                    
                    user.AccountHistory.Add(new AccountHistory
                    {
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        EventType = EventType.SuccessfullLogOut,
                        UserId = user.Id,
                        User = user
                    });
                }
            }
            catch (Exception ex)
            {
                using (StoreDbContext dbContext = new StoreDbContext())
                {
                    user.AccountHistory.Add(new AccountHistory
                    {
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        EventType = EventType.LogOutAttempt,
                        ErrorMessage = ex.Message,
                        User = user,
                        UserId = user.Id
                    });

                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();
                }

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
                if (password != passwordConfirmation)
                {
                    _logger.Log("Passwords don't math");
                    return;
                }

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
                    //TODO упростить выражение - завести переменную
                    if (dbContext.Users.SingleOrDefault(x => x.Login == login) is null)
                    {
                        dbContext.Users.Add(new User
                        {
                            Login = login,
                            Password = hashedPassword,
                            CreatedOn = DateTimeOffset.UtcNow,
                            IsActive = true
                        });
                    }
                    else
                    {
                        throw new SignUpException("This login already exists");
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Log($"[{ex}] {ex.Message}");
                return;
            }
        }
    }
}
