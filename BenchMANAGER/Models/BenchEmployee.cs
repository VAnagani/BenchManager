//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BenchMANAGER.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BenchEmployee
    {
        public int BenchEmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string Practice { get; set; }
        public string SPOC { get; set; }
        public string PotentialAccount { get; set; }
        public string ProjectCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Utilization { get; set; }
        public string LocationType { get; set; }
        public string Location { get; set; }
        public string AssignmentStatus { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ChangedDate { get; set; }
        public string ChangedBy { get; set; }
    }
}
