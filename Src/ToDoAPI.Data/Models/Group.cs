using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Group
    {
        public Group()
        {
            Todos = new HashSet<Todo>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Admin { get; set; }

        public virtual User AdminNavigation { get; set; } = null!;
        public virtual User CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<Todo> Todos { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
