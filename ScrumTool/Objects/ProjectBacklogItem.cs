using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumTool.Objects
{
    public class ProjectBacklogItem
    {
        public enum BacklogItemState
        {
            [Display(Name = "Not Started")]
            NotStarted = 0,
            [Display(Name = "In Progress")]
            InProgress = 1,
            [Display(Name = "Complete")]
            Complete = 2,
            [Display(Name = "Removed")]
            Removed = 10
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public int TaskCount { get; set; }

        public int? Priority { get; set; }
        public BacklogItemState State { get; set; }
    }
}