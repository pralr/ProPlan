using ProjectManager.Enums;
using ProjectManager.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProjectManager.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }

        public string? Linkedln { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        [Display(Name = "Tipo de Cargo")]
        public EnumTipoCargo TipoCargo { get; set; }
        public List<int>? Projetos { get; set; }
        public List<int>? Atividades { get; set; }
        public double ValorBolsa { get; set; }
    }
}
