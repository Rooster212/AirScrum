namespace ScrumTool.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ScrumTask
    {
        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int? Weight { get; set; }

        public string Comments { get; set; }

        public int BacklogItemID { get; set; }

        public int Priority { get; set; }

        public virtual BacklogItem BacklogItem { get; set; }
    }
}
