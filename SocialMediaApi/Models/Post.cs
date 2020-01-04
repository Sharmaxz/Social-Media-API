using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public bool Favorite { get; set; }
        public string Timestamp { get; set; }
        public Client Owner { get; set; }
    }
}