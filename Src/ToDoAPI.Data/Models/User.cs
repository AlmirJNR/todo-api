using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class User
    {
        public User()
        {
            GroupAdminNavigations = new HashSet<Group>();
            GroupCreatedByNavigations = new HashSet<Group>();
            Todos = new HashSet<Todo>();
            Groups = new HashSet<Group>();
        }

        public Guid AuthServerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Group> GroupAdminNavigations { get; set; }
        public virtual ICollection<Group> GroupCreatedByNavigations { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
