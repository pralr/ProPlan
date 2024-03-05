using ProjectManager.Enums;
using ProjectManager.Models;

namespace ProjectManager.ViewModels
{
    public class FeedViewModel
    {
        public string? NomeUsuario { get; set; }
        public string? DescricaoUsuario { get; set; }
        public EnumTipoCargo CargoUsuario { get; set; }

        public string? DescricaoPostagem {get; set;}
        public IEnumerable<PostagemFeed>? Postagens { get; set; }

    }
}
