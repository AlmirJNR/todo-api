using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool? Completed { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? InGroup { get; set; }

        public virtual User CreatedByNavigation { get; set; } = null!;
        public virtual Group? InGroupNavigation { get; set; }
    }
}
