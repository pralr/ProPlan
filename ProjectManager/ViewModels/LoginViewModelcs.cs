using System.ComponentModel.DataAnnotations;

namespace ProjectManager.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo de usuário é obrigatório.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
