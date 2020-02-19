using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Api.Contracts.V1.Requests
{
    public class UpdatePostViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
