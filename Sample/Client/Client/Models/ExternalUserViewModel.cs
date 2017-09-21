using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ExternalUserViewModel
    {
        public string Email { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string Provider { get; set; }
    }
}