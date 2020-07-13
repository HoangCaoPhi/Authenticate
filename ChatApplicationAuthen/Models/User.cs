using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Models
{
    public class User
    {
 
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string LastName { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Email { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string ContactMobile { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string AvatarUrl { get; set; }

        [Column(TypeName = "VARCHAR(64)")]
        public string Password { get; set; }

    }
}
