using System;
using System.Collections.Generic;

namespace Store.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsActive { get; set; }

        #region Navigation properties

        public virtual ICollection<Role> Roles { get; set; }
        #endregion
    }
}
