using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProjectManager.Models
{
    public class PadraoCadastro
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string? Descricao { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        public DateTime DataTermino { get; set; }

        [Display(Name = "Responsável")]
        public string Responsavel {get;set; }

        [Display(Name = "Integrantes")]
        public List<string>? Integrantes { get; set; }

        //Em projetos, considera que o período de vigência da bolsa é o mesmo período do projeto*


        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return "Não informado";

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
