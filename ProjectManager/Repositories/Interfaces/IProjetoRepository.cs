using ProjectManager.Models;

namespace ProjectManager.Repositories.Interfaces
{
    public interface IProjetoRepository
    {
        public IEnumerable<Projeto> Projetos { get; }

        public IEnumerable<Projeto> ProjetosEspecificos(string idUsuario);
        public Dictionary<string, string> ObterIntegrantesPorProjeto(int idProjeto);
        public void AddIntegrante(string idIntegrante, int idProjeto);
        public void DeleteIntegrante(string idIntegrante, int idProjeto);
        public void Add(Projeto projeto);
        public void Update(Projeto projeto);
        public void Delete(Projeto projeto);

    }
}
