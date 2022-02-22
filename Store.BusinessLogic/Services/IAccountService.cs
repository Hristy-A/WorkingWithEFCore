using Store.Data.Entities;

namespace Store.BusinessLogic.Services
{
    public interface IAccountService
    {
        User LogIn(string login, string password);
        void LogOut(User user);
        void Disable(User user);
        void SignUp(string login, string password, string passwordConfirmation);
    }
}
