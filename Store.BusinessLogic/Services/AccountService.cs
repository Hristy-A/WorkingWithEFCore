using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;
using Store.Infrastructure.HashProviders;

namespace Store.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHashProvider _hashProvider;

        public AccountService(IPasswordHashProvider hashProvider)
        {
            _hashProvider = hashProvider ?? throw new ArgumentNullException(nameof(hashProvider));
        }

        public void Disable(User user)
        {
            // TODO:
            throw new NotImplementedException();

            // проверить, есть ли такой пользователь вообще
            // проверить, не деактивирован ли он
        }

        public User Login(string login, string password)
        {
            try
            {
                // TODO: закрыть контекст сразу после загрузки пользователя
                using (var dbContext = new StoreDbContext())
                {
                    var user = dbContext.Users
                        .Include(x => x.Roles)
                        .SingleOrDefault();

                    if (!user.IsActive)
                    {
                        System.Diagnostics.Debug.WriteLine("User is not exist");
                        return null;
                    }

                    if (!_hashProvider.Verify(password, user.Password))
                    {
                        System.Diagnostics.Debug.WriteLine("Wrong password");
                        return null;
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void Logout(User user)
        {
            // TODO: реализовать запись в таблицу истории действий, когда она будет добавлена
            throw new NotImplementedException();
        }

        public void SignUp(string login, string password, string passwordConfirmation)
        {
            // TODO:
            throw new NotImplementedException();

            // проверить логин на уникальность
            // проверить совпадение паролей
            // проверить требования к паролю (В инфраструктуру - интерфейс IPasswordValidator) (план-максимум: посмотри, что такое nuget FluentValidation)

            // не забыть выставить CreatedOn и IsActive
        }
}
