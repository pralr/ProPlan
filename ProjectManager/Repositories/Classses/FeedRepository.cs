using ProjectManager.AppContext;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;

namespace ProjectManager.Repositories.Classses
{
    public class FeedRepository : IFeedRepository
    {
        private readonly AppDbContext _context;

        public FeedRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PostagemFeed> Postagens => _context.PostagensFeed.OrderByDescending(p => p.DataPostagem);

        public void Add(PostagemFeed postagem)
        {
            _context.PostagensFeed.Add(postagem);
            _context.SaveChanges();
        }
    }
}
