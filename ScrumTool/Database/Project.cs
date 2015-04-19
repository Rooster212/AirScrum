namespace ScrumTool.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        public Project()
        {
            BacklogItems = new HashSet<BacklogItem>();
            Sprints = new HashSet<Sprint>();
            Teams = new HashSet<Team>();
            Users = new HashSet<AspNetUser>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public virtual ICollection<BacklogItem> BacklogItems { get; set; }

        public virtual ICollection<Sprint> Sprints { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
