//using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMediaApi.Models
{
    public class Person
    {

        //[Display(Name = "Nickname")]
        //[Required(ErrorMessage = "O nickname é obrigatorio")]
        [Required]
        public string Nickname { get; set; }

        //[Display(Name = "Nome")]
        //[Required(ErrorMessage = "O nome é obrigatorio")]
        public string Name { get; set; }

        //[Display(Name = "Sobrenome")]
        public string Surname { get; set; }
        public string Fullname { get; set; }

        //[BsonDateTimeOptions]
        //[Display(Name = "Data de nascimento")]
        //[Required(ErrorMessage = "A data de nascimento é obrigatorio")]
        public double Birthdate { get; set; }

        //[Required(ErrorMessage = "O email é obrigatorio")]
        [Required]
        public string Email { get; set; }

        //[Required(ErrorMessage = "A senha é obrigatorio")]
        [Required]
        public string Password { get; set; }

    }
}