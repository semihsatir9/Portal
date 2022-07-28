using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class LoginModel
    {

        [DisplayName("Username")]
        [Required]
        [MinLength(6)]
        public string username { get; set; }


        [DisplayName("Password")]
        [Required]

        public int password { get; set; }

    }
}