using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SocialMedia.Controllers;
using SocialMediaApi.DAL;
using SocialMediaApi.Models;


namespace SocialMediaApi.Controllers
{
    //[EnableCors(origins: "https://localhost:44347", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        private SocialMediaContext db = new SocialMediaContext();
        private ImageProcessor image_processor = new ImageProcessor();

        [HttpGet]
        [Route("")]
        public List<Client> GetAll()
        {
            return db.Clients.ToList();
        }

        [HttpGet]
        [Route("{id:int}")]
        public object GetOneById(int id)
        {
            var client = db.Clients.FirstOrDefault(c => c.Id == id);

            if (client is null)
            {
                return "Not Found.";
            }

            return client;
        }

        [HttpGet]
        [Route("{uk}")]
        public object GetOneByNickname(string uk)
        {
            //MSSQL
            //uk = Email or Nickname
            Client client = null;
            HttpContext context = HttpContext.Current;

            if (uk.Contains("@"))
            {
                client = db.Clients.FirstOrDefault(c => c.Email.ToLower().Contains(uk.ToLower()));
            }
            else
            {
                client = db.Clients.FirstOrDefault(c => c.Nickname.ToLower() == uk.ToLower());
                if (client.Profile_pic != null)
                    client.Profile_pic = image_processor.ConvertToBase64(context.Server.MapPath(client.Profile_pic));
                if (client.Cover_pic != null)
                    client.Cover_pic = image_processor.ConvertToBase64(context.Server.MapPath(client.Cover_pic));
            }

            if (client is null)
            {
                return "Not Found.";
            }

            return client;
        }

        [HttpPost]
        [Route("create")]
        public string Create()
        {
            try
            {
                Client client = new Client()
                {
                    Nickname = Request.Headers.GetValues("nickname").FirstOrDefault().ToLower(),
                    Email = Request.Headers.GetValues("email").FirstOrDefault().ToLower(),
                    Name = Request.Headers.GetValues("name").FirstOrDefault(),
                    Surname = Request.Headers.GetValues("surname").FirstOrDefault(),
                    Fullname = Request.Headers.GetValues("name").FirstOrDefault() + " " + Request.Headers.GetValues("surname").FirstOrDefault(),
                    Birthdate = Convert.ToDouble(Request.Headers.GetValues("birthdate").FirstOrDefault()),
                    Password = Request.Headers.GetValues("password").FirstOrDefault(),
                };
                db.Clients.Add(client);
                db.SaveChanges();
                return $"The {client.Nickname} was created.";
            }
            catch
            {
                return "Failed";
            }
        }

        [HttpPut]
        [Route("update/{nickname}")]
        public string Update(string nickname)
        {
            nickname = nickname.ToLower();
            HttpContext context = HttpContext.Current;

            try
            {
                HttpHeaders headers = Request.Headers;
                var client = db.Clients.FirstOrDefault(c => c.Nickname == nickname);

                if (context.Request.Files.Count > 0)
                {
                    for (int i = 0; i < context.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = context.Request.Files[i];
                        if (context.Request.Files.AllKeys[i] == "profile.png")
                            client.Profile_pic = image_processor.CreateImage(context, file, nickname, "profile");
                        if (context.Request.Files.AllKeys[i] == "cover.png")
                            client.Cover_pic = image_processor.CreateImage(context, file, nickname, "cover");

                    }
                }
                if (headers.TryGetValues("Name", out IEnumerable<string> name))
                {
                    client.Name = name.First();
                }
                if (headers.TryGetValues("Surname", out IEnumerable<string> surname))
                {
                    client.Surname = surname.First();
                }
                if (headers.TryGetValues("Bio", out IEnumerable<string> bio))
                {
                    client.Bio = bio.First();
                }
                if (headers.TryGetValues("Birthdate", out IEnumerable<string> birthdate))
                {
                    client.Birthdate = Double.Parse(birthdate.First());
                }
                db.SaveChanges();
            }
            catch
            {
                return "Request invalid, check your credentials or values.";
            }
            return "The Client was updated.";
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public string Delete(int? id)
        {
            try
            {
                var client = db.Clients.First(c => c.Id == id);
                if (client is null)
                {
                    return "This client does not exist.";
                }

                string nickname = client.Nickname;
                db.Clients.Remove(client);
                db.SaveChangesAsync();
                return $"The client ({nickname}) was deleted.";
            }
            catch
            {
                return "This client was not deleted.";
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public string Delete(string id)
        {
            try
            {
                Client client = null;
                if (id.Contains("@"))
                {
                    client = db.Clients.First(c => c.Email == id);
                }
                else
                {
                    client = db.Clients.First(c => c.Nickname == id);
                }

                if (client is null)
                {
                    return "This client does not exist.";
                }

                string nickname = client.Nickname;
                db.Clients.Remove(client);
                db.SaveChangesAsync();
                return $"The client ({nickname}) was deleted.";
            }

            catch
            {
                return "This client was not deleted.";
            }
        }

        [HttpGet]
        [Route("{nickname}/friendship")]
        public List<Friendship> GetFriendship(string nickname)
        {

            return db.Clients.FirstOrDefault(c => c.Nickname == nickname).Friendships.ToList();
        }

        [HttpGet]
        [Route("{nickname}/notification")]
        public List<Notification> GetNotification(string nickname)
        {
            return db.Clients.FirstOrDefault(c => c.Nickname == nickname).Notifications.ToList();
        }

        [HttpGet]
        [Route("{nickname}/featuredphotos")]
        public List<Post> GetGallery(string nickname)
        {
            HttpContext context = HttpContext.Current;

            var client = db.Clients.FirstOrDefault(c => c.Nickname == nickname);
            var featured = client.Gallery.Where(p => p.Favorite == true);
            client.Gallery = new List<Post>();
            if (featured.Count() != 0)
            {
                foreach (var photo in featured)
                {
                    photo.Img = image_processor.ConvertToBase64(context.Server.MapPath(photo.Img));
                    photo.Owner = new Client() { Name = "You do not have permission." };
                    client.Gallery.Add(photo);
                }
            }
            else
            {
                return new List<Post>();
            }

            return client.Gallery.OrderByDescending(p => p.Timestamp).ToList();
        }


        [HttpGet]
        [Route("{nickname}/gallery/{page}")]
        public List<Post> GetGallery (string nickname, int? page)
        {
            HttpContext context = HttpContext.Current;

            int size = 9;
            int page_number = (page ?? 1);

            var client = db.Clients.FirstOrDefault(c => c.Nickname == nickname);

            var photos = client.Gallery.Skip((page_number - 1) * size).Take(size).ToList();
            client.Gallery = new List<Post>();

            foreach (var photo in photos)
            {
                photo.Img = image_processor.ConvertToBase64(context.Server.MapPath(photo.Img));
                photo.Owner = new Client() { Name = "You do not have permission."};
                client.Gallery.Add(photo);
            }

            return client.Gallery.OrderByDescending(p => p.Timestamp).ToList();
        }

        [HttpPost]
        [Route("{nickname}/post")]
        public Post PostUpload(string nickname)
        {
            HttpContext context = HttpContext.Current;
            var timestamp = (UnixTimestamp)DateTime.Now;
            var client = db.Clients.FirstOrDefault(c => c.Nickname == nickname);

            var post = new Post()
            {
                Img = image_processor.CreateImage(context, context.Request.Files[0], nickname, $"{nickname}_{timestamp}"),
                Favorite = false,
                Timestamp = timestamp.ToString(),
                Owner = client,
            };

            db.Posts.Add(post);
            client.Gallery.Add(post);
            db.SaveChanges();
            return post;
        }

    }
}


