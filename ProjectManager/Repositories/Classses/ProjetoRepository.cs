using ProjectManager.AppContext;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;

namespace ProjectManager.Repositories.Classses
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly AppDbContext _context;

        private readonly IUsuarioRepository _usuarioRepository;

        public ProjetoRepository(AppDbContext context, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        public IEnumerable<Projeto> Projetos => _context.Projetos;

		public void Add(Projeto projeto)
		{
			_context.Projetos.Add(projeto);
			Save();
		}

		public void Delete(Projeto projeto)
		{
			_context.Projetos.Remove(projeto);
			Save();
		}

		public void Update(Projeto projeto)
		{
			_context.Projetos.Update(projeto);
			Save();
		}

		public void Save() {
			_context.SaveChanges();
		}

        //Primeiro devemos carregar todos os projetos na memória (com AsEnumerable())
        //Devido à limitação do EF que não suporta o método Contains em uma lista de strings em uma entidade

        public IEnumerable<Projeto> ProjetosEspecificos(string idUsuario)
        {
            var projetos = _context.Projetos.AsEnumerable();
            return projetos.Where(projeto => projeto.Integrantes.Contains(idUsuario)).ToList();
        }

        public void AddIntegrante(string idIntegrante, int idProjeto)
        {
            var projeto = ObterProjetoPorId(idProjeto);
            if (!projeto.Integrantes.Contains(idIntegrante))
            {
                projeto.Integrantes.Add(idIntegrante);
                Save();
            }
        }


        public void DeleteIntegrante(string idIntegrante, int idProjeto)
        {
            var projeto = ObterProjetoPorId(idProjeto);
            if (projeto.Integrantes.Contains(idIntegrante))
            {
                projeto.Integrantes.Remove(idIntegrante);
                Save();
            }
        }

        public Dictionary<string, string> ObterIntegrantesPorProjeto(int idProjeto)
        {
            var projeto = ObterProjetoPorId(idProjeto);
            var integrantes = new Dictionary<string, string>();

            if(projeto != null && projeto.Integrantes != null)
            {
                foreach (var idIntegrante in projeto.Integrantes)
                {
                    var nome = _usuarioRepository.RetornaNomePorId(idIntegrante);
                    integrantes.Add(idIntegrante, nome);
                }
            }
            return integrantes;
        }


        public Projeto ObterProjetoPorId(int idProjeto)
        {
            return _context.Projetos.Where(x => x.Id == idProjeto).FirstOrDefault();
        }
    }
}
