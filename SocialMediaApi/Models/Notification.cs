using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Sended { get; set; }
        public bool Viewed { get; set; }
        public string Text { get; set; }
        public virtual Client Sender { get; set; }
        public virtual Client Recepient { get; set; }
    }
}