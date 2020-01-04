using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Client : Person
    {
        //Methods 
        public int? Id { get; set; }
        public string Cover_pic { get; set; }
        public string Profile_pic { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<Post> Gallery { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
    }
}