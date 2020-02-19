using Community.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.Domain
{
    public enum IssuerType
    {
        Developer , Organization , Company
    }
    public class Post
    {
        public string Id { get; set; }
        public IssuerType IssuerType { get; set; }
        public string IssuerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string Image { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Emotion> Emotions { get; set; }

    }
}
