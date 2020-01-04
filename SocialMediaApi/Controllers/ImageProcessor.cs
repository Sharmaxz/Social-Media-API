using System;
using System.IO;
using System.Web;
using System.Numerics;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace SocialMedia.Controllers
{
    public class ImageProcessor
    {
        HttpContext context;
        public void CheckDirectory(string nickname)
        {
            if (!Directory.Exists(context.Server.MapPath("~/uploads/profiles/" + nickname)))
            {
                Directory.CreateDirectory(context.Server.MapPath("~/uploads/profiles/" + nickname));
            }
        }

        public byte[] ConvertToByte(HttpPostedFile file)
        {
            byte[] file_byte = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                file_byte = binaryReader.ReadBytes(file.ContentLength);
            }
            return file_byte;
        }

        public string ConvertToBase64(string path)
        {
            this.context = HttpContext.Current;

            using (var image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public string CreateImage(HttpContext context, HttpPostedFile file, string nickname, string filename)
        {
            this.context = context;
            var path = $"~/uploads/profiles/{nickname}/{filename}.png";
            if (context.Request.Files.Count > 0)
            {

                CheckDirectory(nickname);
                var file_byte = ConvertToByte(file);

                using (Image image = Image.Load(file_byte))
                {
                    Configuration.Default.ImageFormatsManager.SetEncoder(PngFormat.Instance, new PngEncoder() {
                        CompressionLevel = 6
                    });
                    image.Mutate(img => img.Resize(1500, 350));
                    image.Save(context.Server.MapPath(path));
                }
            }

            return path;
        }

    }
}