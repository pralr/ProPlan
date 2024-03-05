using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManager.Models;
using ProjectManager.Repositories.Classses;
using ProjectManager.Repositories.Interfaces;
using ProjectManager.ViewModels;

namespace ProjectManager.Controllers
{
    public class AtividadeController : Controller
    {
        private readonly IAtividadeRepository _atividadeRepository;
        private readonly IProjetoRepository _projetoRepository;

        public AtividadeController(IAtividadeRepository atividadeRepository, IProjetoRepository projetoRepository)
        {
            _atividadeRepository = atividadeRepository;
            _projetoRepository = projetoRepository;
        }
        public IActionResult Index()
        {
            var atividadesViewModel = new AtividadeProjetoViewModel
            {
                Atividades = _atividadeRepository.Atividades,
                Projeto = new Models.Projeto()
            };

            return View(atividadesViewModel);
        }

        public IActionResult Create(int idProjeto)
        {
            PopulaViewBags(idProjeto);

            ViewBag.IdProjeto = idProjeto;
            return View();
        }

        public IActionResult Edit(int id)
        {
            var atividade = _atividadeRepository.Atividades.FirstOrDefault(p => p.Id == id);
            if (atividade == null)
            {
                return NotFound();
            }

            PopulaViewBags(atividade.ProjetoId);
            return View(atividade);
        }

        [HttpPost]
        public IActionResult Edit(Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                _atividadeRepository.Update(atividade);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id = atividade.ProjetoId });
        }


        [HttpPost]
        public IActionResult Create(Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                PopulaViewBags(atividade.ProjetoId);

                _atividadeRepository.Add(atividade);

                TempData["AtividadeCriada"] = true;

                return RedirectToAction("Details", "Projeto", new { id = atividade.ProjetoId });
            }
            return View(atividade);
        }

        [HttpPost]
        public IActionResult AddIntegrante(string id, Atividade atividade)
        {
            _atividadeRepository.AddIntegrante(id, atividade.Id);

            return View();
        }

        public void PopulaViewBags(int idProjeto)
        {
            var integrantesProjeto = _projetoRepository.ObterIntegrantesPorProjeto(idProjeto) ?? new Dictionary<string, string>();

            ViewBag.IntegrantesProjeto = integrantesProjeto
                                                    .Select(i => new SelectListItem
                                                    {
                                                        Value = i.Key.ToString(),
                                                        Text = i.Value
                                                    })
                                                    .ToList();

        }

        public IActionResult AddIntegrante(string idUsuario, int id)
        {
            _projetoRepository.AddIntegrante(idUsuario, id);

            return RedirectToAction("Edit", new { id = id });
        }

        public IActionResult DeleteIntegrante(string idUsuario, int id)
        {
            _projetoRepository.DeleteIntegrante(idUsuario, id);

            return RedirectToAction("Edit", new { id = id });
        }
    }
}
