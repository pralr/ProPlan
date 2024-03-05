using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class PostagemFeed
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? NomeUsuario { get; set; }
        public string? UsuarioId { get; set; }
        public DateTime DataPostagem { get; set;}

        [NotMapped]
        public List<PostagemFeed> TodasPostagens { get; }
    }
}
