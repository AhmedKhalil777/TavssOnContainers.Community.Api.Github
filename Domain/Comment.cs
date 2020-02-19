using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Api.Domain
{
    public class Comment
    {
        public string Id { get; set; }
        public string SubscriperId { get; set; }
        public string CommentText { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;

    }
}
