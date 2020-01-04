using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using SocialMediaApi.DAL;
using SocialMediaApi.Models;
namespace SocialMediaApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/friendship")]
    public class FriendshipController : ApiController
    {
        private SocialMediaContext db = new SocialMediaContext();

        // GET: Friendship
        [HttpGet]
        [Route("")]
        public List<Friendship> GetAll()
        {
            return db.Friendships.ToList();
        }

        [HttpPost]
        [Route("create/{sender}/{recepient}")]
        public string Create(string recepient, string sender)
        {
            try
            {
                var client_recepient = db.Clients.FirstOrDefault(c => c.Nickname == recepient);
                var client_sender = db.Clients.FirstOrDefault(c => c.Nickname == sender);
                HttpHeaders headers = Request.Headers;

                var notification = new Notification()
                {
                    Type = "friendship",
                    Sended = true,
                    Viewed = false,
                    Text = $"The {client_sender.Name} would like to be your friend!",
                    Sender = client_sender,
                    Recepient = client_recepient,
                };

                var friendship = new Friendship()
                {
                    Recepient = client_recepient.Nickname,
                    Sender = client_sender.Nickname,
                    Accepted = false,
                    Timestamp = headers.GetValues("TimeStamp").FirstOrDefault(),
                    Clients = new List<Client> { client_recepient, client_sender },
                    Notification = notification,
                };

                friendship = db.Friendships.Add(friendship);
                client_recepient.Friendships.Add(friendship);
                client_sender.Friendships.Add(friendship);
                client_recepient.Notifications.Add(notification);
                db.SaveChanges();
            }
            catch
            {
                return "Failed!";
            }
            return "The friendship was created!";
        }

        [HttpDelete]
        [Route("delete/{sender}/{recepient}")]
        public string Delete(string sender, string recepient)
        {
            sender = sender.ToLower();
            recepient = recepient.ToLower();

            try
            {
                var client_recepient = db.Clients.FirstOrDefault(c => c.Nickname == recepient);
                var client_sender = db.Clients.FirstOrDefault(c => c.Nickname == sender);
                var friendship = db.Friendships.FirstOrDefault(f => f.Sender == sender && f.Recepient == recepient);
                var notification = db.Notifications.FirstOrDefault(n => n.Sender.Nickname == sender && n.Recepient.Nickname == recepient);

                db.Friendships.Remove(friendship);
                db.Notifications.Remove(notification);
                db.SaveChanges();
            }
            catch
            {
                return "Failed!";
            }
            return "The friendship was deleted!";
        }

        [HttpPost]
        [Route("accept/{id:int}")]
        public string AcceptFriendship(int id)
        {
            try
            {
                var friendship = db.Friendships.FirstOrDefault(f => f.Id == id);
                friendship.Accepted = true;
                friendship.Notification.Sended = false;
                db.SaveChanges();
            }
            catch
            {
                return "Failed!";
            }
            return "The friend request was accepted!";
        }
    }
}