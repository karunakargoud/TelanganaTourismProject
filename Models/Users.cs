using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TouristPlacesProject.Models
{
    public class Users
    {
        [Key]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set; }
    }
}