using ProjectManager.Models;

namespace ProjectManager.ViewModels
{
    public class AtividadeProjetoViewModel
    {
        public Projeto? Projeto{ get; set; }

        public IEnumerable<Atividade>? Atividades { get; set; }
    }
}
