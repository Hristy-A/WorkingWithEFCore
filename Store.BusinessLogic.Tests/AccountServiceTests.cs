using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Store.BusinessLogic.Exceptions;
using Store.BusinessLogic.Services;
using Store.Data;
using Store.Data.Entities;
using Store.Infrastructure.HashProviders;

namespace Store.BusinessLogic.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void SuccessfullSignUp()
        {
            //Arrange

            //Конфигурируем датасет юзеров таким образом, что он не содержит данных (т.е. пользователей в БД нет)
            var usersSetMock = DbSetFaker.MockDbSet(Enumerable.Empty<User>());

            var dbContextMock = new Mock<StoreDbContext>();
            //Говорим, что при обращении к имитации датаконтекста, нужно вернуть имитацию пустого набора пользователей
            dbContextMock.Setup(c => c.Users).Returns(usersSetMock.Object);

            //Конфигурируем "ничего не делающие" зависимости - нам и правда не важно, как они себя будт вести, т.к.
            //поведение тестируемого компонента (AccountService) в конкретном тестируемом сценарии (Успешная регистрация)
            //по задумке не должно зависеть
            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            //Собираем сервис, передавая имитации зависимостей
            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            string login = "login";
            string password = "password";

            //Act
            accountService.SignUp(login, password, password);

            //Assert
            // проверяем, что мы "добавляем" нового пользователя и что "сохраняем" его в БД
            // на самом деле никакой БД нет и никакое сохранение не происходит. Но нам главное удостовериться в том,
            // что в реальном приложении, когда база существует и исправно работает, мы отправим туда нового пользователя 
            // на сохранение.
            dbContextMock.Verify(x => x.Users.Add(It.IsAny<User>()), Times.Once());
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void SignUp_WhenLoginIsBusy_ShouldFail()
        {
            //Arrange
            var usersSetMock = DbSetFaker.MockDbSet(new List<User>()
            {
                new User() { Login = "busyLogin" }
            });

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(c => c.Users).Returns(usersSetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            string login = "busyLogin";
            string password = "password";

            //Act
            Assert.ThrowsException<SignupException>(() =>
            {
                accountService.SignUp(login, password, password);
            });

            //Assert
            dbContextMock.Verify(x => x.Users.Add(It.IsAny<User>()), Times.Never());

            //т.к. метод Add принимает экземпляр класса User, с помощью конструкции It.IsAny<User>() мы говорим,
            //что неважно с каким именно параметром этот метод вызовется. Главное - факт вызова. А в нашем случае - факт
            //отсутствия вызова.
        }

        [TestMethod]
        public void SignUp_WhenPasswordsDontMatch_ShouldFail()
        {
            //Arrange
            var usersSetMock = DbSetFaker.MockDbSet(Enumerable.Empty<User>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(c => c.Users).Returns(usersSetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            string login = "login";
            string password = "password";
            string passwordConfirmation = "password1";

            //Act
            Assert.ThrowsException<SignupException>(() =>
            {
                accountService.SignUp(login, password, passwordConfirmation);
            });

            //Assert
            dbContextMock.Verify(x => x.Users.Add(It.IsAny<User>()), Times.Never());
        }

        [TestMethod]
        public void SuccessfullLogin()
        {
            //Arrange
            string login = "login";
            string password = "password";
            string passwordHash = "passwordHash";

            var users = new List<User>()
            {
                new User()
                {
                    Login = login,
                    Password = passwordHash,
                    Disabled = false
                }
            };

            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(Enumerable.Empty<AccountHistory>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(c => c.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(c => c.AccountHistories).Returns(accountHistorySetMock.Object);

            var passwordHashProviderStub = new Mock<IPasswordHashProvider>();
            passwordHashProviderStub.Setup(x => x.Verify(password, passwordHash)).Returns(true);
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProviderStub.Object, logger);

            //Act
            User user = accountService.LogIn(login, password);

            //Assert
            Assert.IsNotNull(user);
        }
    }
}

// TODO
//User LogIn(string login, string password);
//  Good way:
//		-User object is returned (OK)
//   Bad way:
//      - login not found
//		- incorrect password
//		- user is disabled

//void LogOut(User user);
//   Bad way:
//		- user not found
//		- user is disabled

//void Disable(User user);
//  Good way:
//		-user.Disabled is set to true

//    Bad way:
//		- user not found
//		- user is already disabled
