using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OOP_CA_Macintosh.Models
{
    public class User
    {
         [Required]
        public string Username { get; set; }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        //password between 6 and 12, with 1 number and 1 uppercase
        [RegularExpression(@"/^(?=.*[A-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@])(?!.*[iIoO])\S{6,12}$/")]
        public string Password { get; set; }
        public string Role { get; set; }
        public string AccessLevel { get; set; }
    }
}
