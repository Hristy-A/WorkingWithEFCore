using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.PasswordValidators
{
    public interface IPasswordValidator
    {
        // HACK: настроить и обсудить интерфейс, для валидации паролей, как тут дложны быть члены и на что проверять пароль
        bool ValidatePassword(string password);
    }
}
