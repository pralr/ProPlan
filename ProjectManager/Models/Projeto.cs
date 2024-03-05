using ProjectManager.Enums;

namespace ProjectManager.Models
{
    public class Projeto : PadraoCadastro
    {
        
        public EnumStatusProjeto Status { get; set; }
        public List<Atividade>? Atividades { get; set; }

    }
}
