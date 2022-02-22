using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.PasswordValidators
{
    public interface IPasswordValidator
    {
        bool ValidatePassword(string password);
    }
}
