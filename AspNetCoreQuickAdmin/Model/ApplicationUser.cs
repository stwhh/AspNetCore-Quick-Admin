using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ApplicationUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        //工号
        public string UserNo { get; set; }

        public string EnUserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
