using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Recepient { get; set; }
        [Required]
        public string Timestamp { get; set; }
        [Required]
        public bool Accepted { get; set; }

        [Required]
        public virtual ICollection<Client> Clients { get; set; }

        [Required]
        public virtual Notification Notification { get; set; }

    }
}