using System;

namespace Store.Data.Entities
{
    public class AccountHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public EventType EventType { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public string ErrorMessage { get; set; }

        #region Navigation Properties

        public virtual User User { get; set; }
        #endregion
    }
}
