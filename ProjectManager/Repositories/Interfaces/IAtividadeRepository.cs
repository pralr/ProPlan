using ProjectManager.Models;

namespace ProjectManager.Repositories.Interfaces
{
    public interface IAtividadeRepository
    {
        IEnumerable<Atividade> Atividades { get; }
        IEnumerable<Atividade> ObterAtividadesAssociadasAProjeto(int idProjeto);
        Atividade ObtemAtividadePorId(int id);
        public void AddIntegrante(string idIntegrante, int idAtividade);
        public void DeleteIntegrante(string idIntegrante, int idAtividade);
        public void Add(Atividade atividade);
        public void Update(Atividade atividade);
        public void Delete(Atividade atividade);
    }
}
