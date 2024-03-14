using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models.ViewModels.AppUsers.PureVms
{
    public class UserSignInRequestModel
    {
        [Required(ErrorMessage ="Kullanıcı ismi zorunludur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage =("Şifre zorunludur.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; } // Burası kullanıcının başta gitmek istediği url'i tutar. Kişi eğer Login olmadan bir sayfaya gitmek isterse ve o sayfa authorization'a sahipse kişi engellenir ve login'e atılır. Login ile istenilen role'u kanıtlarsa bu adrese otomatik yönlendirilir.

    }
}
