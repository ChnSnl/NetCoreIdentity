using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models.ViewModels.AppUsers.PureVms
{
    public class UserRegisterRequestModel
    {
        [Required(ErrorMessage ="{0} girilmesi zorunludur.")]
        [Display(Name = "Kullanıcı İsmi")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email formatında giriş yapınız")]
        public string Email { get; set; }

        [Required(ErrorMessage ="{0} zorunludur")]
        [MinLength(3, ErrorMessage =" Minimum {0} karakter girilmesi gerekmektedir.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Parolalar uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
