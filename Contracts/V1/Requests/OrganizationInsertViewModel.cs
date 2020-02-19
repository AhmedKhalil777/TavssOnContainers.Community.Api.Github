using Commuinity.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Api.Contracts.V1.Requests
{
    public class OrganizationInsertViewModel
    {
        public string CId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public OrganizationType OType { get; set; }
        
    }
}
