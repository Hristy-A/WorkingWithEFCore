using System;
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
            List<User> users = new()
            {
                new User
                {
                    Login = "busyLogin"
                }
            };

            var usersSetMock = DbSetFaker.MockDbSet(users);

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

            List<User> users = new()
            {
                new User
                {
                    Login = login,
                    Password = passwordHash,
                    Disabled = false
                }
            };

            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(c => c.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(c => c.AccountHistory).Returns(accountHistorySetMock.Object);

            var passwordHashProviderStub = new Mock<IPasswordHashProvider>();
            passwordHashProviderStub.Setup(x => x.Verify(password, passwordHash)).Returns(true);
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProviderStub.Object, logger);

            //Act
            User user = accountService.LogIn(login, password);

            //Assert
            Assert.IsNotNull(user);
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void LogIn_WhenLoginNotFound_ShouldFail()
        {
            // Arange
            string login = "login";
            string password = "pasword";

            var usersSetMock = DbSetFaker.MockDbSet(Enumerable.Empty<User>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);

            IPasswordHashProvider passwordHashProviderStub = Mock.Of<IPasswordHashProvider>();
            ILogger loggerProviderStub = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProviderStub, loggerProviderStub);

            // Act
            // Assert
            Assert.ThrowsException<LoginException>(() => accountService.LogIn(login, password));
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Never());
        }

        [TestMethod]
        public void LogIn_WhenPasswordIncorrect_ShouldFail()
        {

            // Arange
            string login = "login";
            string password = "incorrectPassword";

            List<User> users = new()
            {
                new User
                {
                    Login = login,
                    Password = "password",
                    Disabled = false
                }
            };

            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            var passwordHashProvider = new Mock<IPasswordHashProvider>();
            passwordHashProvider.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Func<string, string, bool>((x, y) => x == y));

            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider.Object, logger);

            // Act
            // Assert
            Assert.ThrowsException<LoginException>(() => accountService.LogIn(login, password), "Wrong password");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void Login_WhenUserDisabled_ShouldThrowLoginException()
        {

            // Arange
            string login = "login";
            string password = "password";

            List<User> users = new()
            {
                new User
                {
                    Login = login,
                    Password = password,
                    Disabled = true
                }
            };

            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            // Assert
            Assert.ThrowsException<LoginException>(() => accountService.LogIn(login, password), "User was deleted");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void LogOut_WhenUserNotFound_ShouldThrowLogoutException()
        {
            // Arange
            int userId = 1;

            var usersSetMock = DbSetFaker.MockDbSet(Enumerable.Empty<User>());
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            // Assert
            Assert.ThrowsException<LogoutException>(() => accountService.LogOut(userId), "User not found");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void LogOut_WhenUserIsDisabled_ShouldThrowLogoutException()
        {
            // Arange
            int userId = 1;

            List<User> users = new()
            {
                new User
                {
                    Id = userId,
                    Login = "login",
                    Disabled = true
                }
            };
            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            // Assert
            Assert.ThrowsException<LogoutException>(() => accountService.LogOut(userId), "User was deleted");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void SuccessfullDisable()
        {
            // Arange
            int userId = 1;

            List<User> users = new()
            {
                new User
                {
                    Id = userId,
                    Login = "login",
                    Disabled = false
                }
            };
            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            accountService.Disable(userId);

            // Assert
            Assert.IsTrue(users.First().Disabled);
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Once());
        }

        [TestMethod]
        public void Disable_WhenUserNotFound_ThrowDisableException()
        {
            // Arange
            int userId = 1;

            var usersSetMock = DbSetFaker.MockDbSet(Enumerable.Empty<User>());
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            // Assert
            Assert.ThrowsException<DisableException>(() => accountService.Disable(userId), "User not found");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Never());
        }

        [TestMethod]
        public void Disable_WhenUserIsAlreadyDisabled_ShouldThrowDisableException()
        {
            // Arange
            int userId = 1;

            List<User> users = new()
            {
                new User
                {
                    Id = userId,
                    Login = "login",
                    Disabled = true
                }
            };
            var usersSetMock = DbSetFaker.MockDbSet(users);
            var accountHistorySetMock = DbSetFaker.MockDbSet(new List<AccountHistoryEntry>());

            var dbContextMock = new Mock<StoreDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersSetMock.Object);
            dbContextMock.Setup(x => x.AccountHistory).Returns(accountHistorySetMock.Object);

            IPasswordHashProvider passwordHashProvider = Mock.Of<IPasswordHashProvider>();
            ILogger logger = Mock.Of<ILogger>();

            IAccountService accountService = new AccountService(dbContextMock.Object, passwordHashProvider, logger);

            // Act
            // Assert
            Assert.ThrowsException<DisableException>(() => accountService.Disable(userId), "User is already disabled");
            dbContextMock.Verify(x => x.Users, Times.Once());
            dbContextMock.Verify(x => x.AccountHistory, Times.Never());
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
