using System;

namespace Store.Data.Entities
{
    public class AccountHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        // по тз нужно добавить логин, но я не совсем понимаю зачем
        // возможно это тот логин, который пользователь вводит при
        // неправильной попытке входа. Решил пока не добавлять.
        //public string UserLogin { get; set; }
        public EventType EventType { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public string ErrorMessage { get; set; }

        #region Navigation Properties

        public virtual User User { get; set; }
        #endregion
    }
}
