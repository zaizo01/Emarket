using System.ComponentModel.DataAnnotations;

namespace Emarket.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Valor requerido.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Valor requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
