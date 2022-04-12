using Store.Data.Entities;

namespace Store.BusinessLogic.Services
{
    public interface IAccountService
    {
        User LogIn(string login, string password);
        void LogOut(int userId);
        void Disable(int userId);
        void SignUp(string login, string password, string passwordConfirmation);
    }
}
