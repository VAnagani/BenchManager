using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BenchMANAGER.ViewModels
{
    public class BenchEmployeeViewModel
    {
        [Key]
        public int BenchEmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Practice { get; set; }
        public string SPOC { get; set; }
        public string PotentialAccount { get; set; }
        public string ProjectCode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Utilization { get; set; }
        public string LocationType { get; set; }
        public string Location { get; set; }
        public string AssignmentStatus { get; set; }
        public string Comments { get; set; }
        
    }
}