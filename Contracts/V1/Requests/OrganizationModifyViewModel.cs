using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Api.Contracts.V1.Requests
{
    public class OrganizationModifyViewModel
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
