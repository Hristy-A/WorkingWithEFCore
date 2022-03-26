using System.ComponentModel;

namespace Store.Data.Entities
{
    public enum EventType
    {
        [Description("Successfull login")]
        SuccessfullLogin,
        [Description("Successfull logout")]
        SuccessfullLogout,
        [Description("Failed login attempt")]
        LoginAttempt,
        [Description("Failed logout attempt")]
        LogoutAttempt,
        [Description("Account disabled")]
        Disabled,
    }
}
