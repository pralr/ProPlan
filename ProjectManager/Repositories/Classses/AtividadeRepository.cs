using Microsoft.EntityFrameworkCore;
using ProjectManager.AppContext;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;

namespace ProjectManager.Repositories.Classses
{
    public class AtividadeRepository : IAtividadeRepository
    {
        private readonly AppDbContext _context;
        public AtividadeRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Atividade> Atividades => _context.Atividades.Include(p => p.Projeto);

        public void Add(Atividade atividade)
        {
            _context.Atividades.Add(atividade);
            Save();
        }

        public void Delete(Atividade atividade)
        {
            _context.Atividades.Remove(atividade);
            Save();
        }

        public Atividade ObtemAtividadePorId(int id)
        {
            return _context.Atividades.FirstOrDefault(atividade => atividade.Id == id);
        }

        public IEnumerable<Atividade> ObterAtividadesAssociadasAProjeto(int idProjeto)
        {
            return Atividades.Where(atividade => atividade.ProjetoId == idProjeto);
        }

        public void Update(Atividade atividade)
        {
            _context.Atividades.Update(atividade);
            Save();
        }

        public void AddIntegrante(string idIntegrante, int idProjeto)
        {
            var atividade = ObterAtividadePorId(idProjeto);
            if (!atividade.Integrantes.Contains(idIntegrante))
            {
                atividade.Integrantes.Add(idIntegrante);
                Save();
            }
        }


        public void DeleteIntegrante(string idIntegrante, int idProjeto)
        {
            var atividade = ObterAtividadePorId(idProjeto);
            if (atividade.Integrantes.Contains(idIntegrante))
            {
                atividade.Integrantes.Remove(idIntegrante);
                Save();
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        public Atividade ObterAtividadePorId(int idAtividade)
        {
            return _context.Atividades.Where(x => x.Id == idAtividade).FirstOrDefault();
        }
    }
}
