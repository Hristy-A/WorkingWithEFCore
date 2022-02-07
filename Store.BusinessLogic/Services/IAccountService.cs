using Store.Data.Entities;

namespace Store.BusinessLogic.Services
{
    public interface IAccountService
    {
        User Login(string login, string password);
        void Logout(User user);
        void Disable(User user);
        void SignUp(string login, string password, string passwordConfirmation);
    }
}
