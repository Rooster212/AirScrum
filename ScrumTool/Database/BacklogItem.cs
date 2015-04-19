namespace ScrumTool.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using ScrumTool.Objects;

    [Table("BacklogItem")]
    public partial class BacklogItem
    {
        public BacklogItem()
        {
            Tasks = new HashSet<ScrumTask>();
            Sprints = new HashSet<Sprint>();
        }

        public int ProjectID { get; set; }

        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public string Comments { get; set; }

        public int? Priority { get; set; }

        public ProjectBacklogItem.BacklogItemState State { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<ScrumTask> Tasks { get; set; }

        public virtual ICollection<Sprint> Sprints { get; set; }
    }
}
