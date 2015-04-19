namespace ScrumTool.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sprint")]
    public partial class Sprint
    {
        public Sprint()
        {
            BacklogItems = new HashSet<BacklogItem>();
        }

        public int ID { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<BacklogItem> BacklogItems { get; set; }
    }
}
