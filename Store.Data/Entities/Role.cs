using System.Collections.Generic;

namespace Store.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string ShortName { get; set; }

        #region Navigation properties

        public virtual ICollection<User> Users { get; set; }
        #endregion
    }
}
