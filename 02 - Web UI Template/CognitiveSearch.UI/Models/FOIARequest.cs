using Azure.Core;
using Microsoft.VisualBasic;
using System;

namespace CognitiveSearch.UI.Models
{
    public class FOIARequest
    {
        public FOIARequest() { }
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string CaseType { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime PerfectedDate { get; set; }
        public DateTime RequestStartDate { get; set; }
        public DateTime RequestEndDate { get; set; }
        public string Classification { get; set; }
        public string Caveat { get; set; }
        public string RequestorFullName { get; set; }
        public string RequestorOrganization { get; set; }
        public string Background { get; set; }
        public string Description { get; set; }
        public string RequestDocumentId { get; set; }
        public string AssignedOfficerName { get; set; }
    }
}
