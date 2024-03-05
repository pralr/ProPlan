using ProjectManager.Models;

namespace ProjectManager.Repositories.Interfaces
{
    public interface IFeedRepository
    {
        public IEnumerable<PostagemFeed> Postagens { get; }
        public void Add(PostagemFeed postagem);
    }
}
