using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Like
    {
        public int Id { get; set; }
        public Client Owner { get; set; }
    }
}