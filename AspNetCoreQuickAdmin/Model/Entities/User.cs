using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Model.Entities.Interface;

namespace Model.Entities
{
    public class User : FullAudited
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string EnUserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

    }
}
