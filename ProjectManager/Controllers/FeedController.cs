using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using ProjectManager.Repositories.Classses;
using ProjectManager.Repositories.Interfaces;
using ProjectManager.ViewModels;

namespace ProjectManager.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedRepository _feedRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public FeedController(IFeedRepository feedRepository, UserManager<ApplicationUser> userManager)
        {
            _feedRepository = feedRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var usuario = _userManager.GetUserAsync(User).Result;

            var feedViewModel = new FeedViewModel
            {
                NomeUsuario = usuario.Nome,
                DescricaoUsuario = usuario.Descricao,
                Postagens = _feedRepository.Postagens.ToList()
            };

            return View(feedViewModel);
        }

        public IActionResult Add(FeedViewModel postagemFeed)
        {
            if (ModelState.IsValid)
            {
                var usuario = _userManager.GetUserAsync(User).Result;

                PostagemFeed postagem = new PostagemFeed
                {
                    Descricao = postagemFeed.DescricaoPostagem,
                    NomeUsuario = usuario.Nome,
                    DataPostagem = DateTime.UtcNow
                };

                _feedRepository.Add(postagem);
                return RedirectToAction("Index");
            }
            return View(postagemFeed);
        }

    }
}
