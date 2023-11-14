using System.ComponentModel.DataAnnotations;

namespace LogisticMVC.Models.LoginModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "FirmNumber is required")]
        public int FirmNumber { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
