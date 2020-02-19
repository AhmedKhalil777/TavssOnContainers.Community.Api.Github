using Community.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.Domain
{
    public enum OrganizationType
    {
        Company , Free
    }
    public class Organization
    {
        [Required]
        public string Id { get; set; }
        public string CId { get; set; }
        public OrganizationType OType { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public List<Developer> Developers { get; set; }
    }
}
