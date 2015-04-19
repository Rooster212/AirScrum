using Microsoft.AspNet.Identity.EntityFramework;

namespace ScrumTool.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Team")]
    public partial class Team
    {
        public Team()
        {
            Projects = new HashSet<Project>();
            Users = new HashSet<AspNetUser>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
