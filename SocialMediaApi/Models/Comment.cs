using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Like> Like { get; set; }
        public string Timestamp { get; set; }
        public Client Sender { get; set; }
        public Client Recepient { get; set; }
    }
}