using ProjectManager.Models;

namespace ProjectManager.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<ApplicationUser> Usuarios { get; }
        ApplicationUser GetUsuarioById(string id);
        ApplicationUser GetUsuarioByUserName(string username);
        public string RetornaNomePorId(string id);

        public void AddIntegrante(string idIntegrante, int idProjeto);

        void Add(ApplicationUser usuario);
        void Update(ApplicationUser usuario);
        void Delete(ApplicationUser usuario);
    }
}
