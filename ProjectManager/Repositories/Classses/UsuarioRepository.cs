using ProjectManager.AppContext;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;

namespace ProjectManager.Repositories.Classses
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> Usuarios => _context.Usuarios;

        public void Add(ApplicationUser usuario)
        {
            throw new NotImplementedException();
        }

        public void Delete(ApplicationUser usuario)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetUsuarioById(string id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetUsuarioByUserName(string username)
        {
            return _context.Usuarios.FirstOrDefault(x => x.UserName == username);
        }

        public void Update(ApplicationUser usuario)
        {
            throw new NotImplementedException();
        }

        public void AddIntegrante(string idIntegrante, int idProjeto)
        {
            var projeto = _context.Projetos.Where(x => x.Id == idProjeto).FirstOrDefault();
            projeto.Integrantes.Add(idIntegrante);
            Save();
        }

        public string RetornaNomePorId(string id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == id);
            return usuario.Nome + " " + usuario.Sobrenome;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
