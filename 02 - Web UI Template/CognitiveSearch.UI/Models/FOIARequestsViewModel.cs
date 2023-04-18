using System.Collections.Generic;

namespace CognitiveSearch.UI.Models
{
    public class FOIARequestsViewModel
    {
        public FOIARequestsViewModel()
        {
            FOIARequests = new List<FOIARequest>();
        }
        public List<FOIARequest> FOIARequests { get; set; }
    }
}
