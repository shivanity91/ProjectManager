using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class ManagerModel
    {
        public List<ProjectInfo> data { get; set; }
    }

    public class ProjectInfo
    {
        public string ProjectName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SourcePath { get; set; }
        public string Comments { get; set; }
    }

}