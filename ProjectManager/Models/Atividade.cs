using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Atividade : PadraoCadastro
    {
        public Dictionary<int, string> Tarefas = new Dictionary<int, string>();

        [Display(Name = "Código do Projeto")]
        public int ProjetoId { get; set; } //O id do projeto será armazenado no banco, mas somente ele

        public virtual Projeto? Projeto { get; set; } //Não vai ser criado na tabela, só será usado para definir o relacionamento entre lanche e categoria
    }
}
