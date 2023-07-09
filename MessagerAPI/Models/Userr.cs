using System.ComponentModel.DataAnnotations;
namespace MessagerAPI.Models
{
    public class Userr
    {


        [Key]
        public int UserId { get; set; }


        [Required]
        public string Username { get; set; } = String.Empty;

        [Required]
        public string Email { get; set; } = String.Empty;


    }
}