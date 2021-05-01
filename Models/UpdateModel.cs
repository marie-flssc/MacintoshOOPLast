using System.ComponentModel.DataAnnotations;
namespace OOP_CA_Macintosh.Models
{
    public class UpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}