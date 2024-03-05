using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Emit;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;
using ProjectManager.ViewModels;
using System.ComponentModel;
using System.Reflection;

namespace ProjectManager.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IAtividadeRepository _atividadeRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjetoController(IProjetoRepository projetoRepository,
                          IUsuarioRepository usuarioRepository,
                          IAtividadeRepository atividadeRepository,
                          UserManager<ApplicationUser> userManager)
        {
            _projetoRepository = projetoRepository;
            _usuarioRepository = usuarioRepository;
            _atividadeRepository = atividadeRepository;
            _userManager = userManager;
        }
        public struct Constantes
        {
            public const string SUCESSO_CADASTRO_ATIVIDADE = "Atividade criada com sucesso!";
            public const string SUCESSO_CADASTRO_PROJETO = "Projeto criado com sucesso!";
            public const string FALHA_CADASTRO_PROJETO = "Somente um professor ou pesquisador pode criar um novo projeto.";
        }

        public IActionResult Index()
        {
            var projetos = _projetoRepository.Projetos;

            if(TempData["AutorizaCriacaoProjeto"] != null)
            {
                TempData["FalhaCadastro"] = Constantes.FALHA_CADASTRO_PROJETO;
            }

            if (TempData["ProjetoCriado"] != null)
            {
                TempData["SucessoCadastro"] = Constantes.SUCESSO_CADASTRO_PROJETO;
            }

            return View(projetos);
        }

        public ViewResult BuscarProjeto(string projeto)
        {
            IEnumerable<Projeto> projetos; 
            string descricaoTela = string.Empty;

            if (string.IsNullOrEmpty(projeto))
            {
                projetos = _projetoRepository.Projetos;
            }
            else
            {
                projetos = _projetoRepository.Projetos.Where(p => p.Descricao.Contains(projeto));
            }

            if (!projetos.Any())
                descricaoTela = "Não foi encontrado nenhum projeto.";

            return View("Index", projetos);
        }

		public IActionResult Create()
		{
            PopulaViewBags(0);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Projeto projeto)
        {
            var usuario = _userManager.GetUserAsync(User).Result;

            var cargosPermitidos = new[] { Enums.EnumTipoCargo.PESQUISADOR, Enums.EnumTipoCargo.PROFESSOR };

            if (!cargosPermitidos.Contains(usuario.TipoCargo))
            {
                TempData["AutorizaCriacaoProjeto"] = false;
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var integranteResponsavel = projeto.Responsavel;
                var idResponsavel = _usuarioRepository.GetUsuarioByUserName(integranteResponsavel).Id;

                if(projeto.Integrantes == null)
                    projeto.Integrantes = new List<string>();

                projeto.Integrantes.Add(idResponsavel);
                
				_projetoRepository.Add(projeto);

                TempData["ProjetoCriado"] = true;

                return RedirectToAction("Index");
			}
			return View(projeto);
		}

		public IActionResult Edit(int id)
		{
            PopulaViewBags(id);

            var projeto = _projetoRepository.Projetos.FirstOrDefault(p => p.Id == id);
			if (projeto == null)
			{
				return NotFound();
			}
			return View(projeto);
		}

		[HttpPost]
		public IActionResult Edit(Projeto projeto)
		{
			if (ModelState.IsValid)
			{
                var projetoSalvo = _projetoRepository.Projetos.FirstOrDefault(p => p.Id == projeto.Id);

                projetoSalvo.Status = projeto.Status;
                projetoSalvo.Descricao = projeto.Descricao;
                projetoSalvo.DataInicio = projeto.DataInicio;
                projetoSalvo.DataTermino = projetoSalvo.DataTermino;


                
				_projetoRepository.Update(projetoSalvo);
				return RedirectToAction("Index");
			}
			return View(projeto);
		}

        public async Task<IActionResult> Details(int id)
        {
           
            var projeto = _projetoRepository.Projetos.FirstOrDefault(p => p.Id == id);

            if (projeto == null)
            {
                return NotFound();
            }

            PopulaViewBags(id);

            ViewBag.StatusProjeto = projeto.Status;

            var atividades = _atividadeRepository.ObterAtividadesAssociadasAProjeto(id);

            foreach(var atvd in atividades)
            {
                var user = await _userManager.FindByIdAsync(atvd.Responsavel);
                atvd.Responsavel = user.Nome + " " + user.Sobrenome;
            }

            AtividadeProjetoViewModel atividadeProjetoViewModel = new AtividadeProjetoViewModel
            {
                Projeto = projeto,
                Atividades = atividades
            };

            if (TempData["AtividadeCriada"] != null)
            {
                TempData["SucessoCadastro"] = Constantes.SUCESSO_CADASTRO_ATIVIDADE;
            }

            return View(atividadeProjetoViewModel);
        }

        [HttpGet]
        public IActionResult ProjetosParticipantes(string id)
        {
            var projetos = _projetoRepository.ProjetosEspecificos(id);
            return View(projetos);
        }

        public void PopulaViewBags(int idProjeto)
        {
			ViewBag.Status = Enum.GetValues(typeof(ProjectManager.Enums.EnumStatusProjeto))
						 .Cast<ProjectManager.Enums.EnumStatusProjeto>()
						 .Select(e => new SelectListItem
						 {
							 Value = ((int)e).ToString(),
							 Text = GetDescription(e)
						 })
						 .ToList();

            var integrantesProjeto = _projetoRepository.ObterIntegrantesPorProjeto(idProjeto) ?? new Dictionary<string, string>();

            ViewBag.IntegrantesProjeto = integrantesProjeto
                                                    .Select(i => new SelectListItem
                                                    {
                                                        Value = i.Key.ToString(),
                                                        Text = i.Value
                                                    })
                                                    .ToList();
        }

        public string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
        public IActionResult Delete(int id)
        {
            var projeto = GetProjeto(id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var projeto = GetProjeto(id);
            if (projeto == null)
            {
                return NotFound();
            }

            _projetoRepository.Delete(projeto);
            return RedirectToAction("Index");
        }

        private Projeto GetProjeto(int id)
        {
            return _projetoRepository.Projetos.FirstOrDefault(p => p.Id == id);
        }

        public IActionResult AddIntegrante(string idUsuario, int id)
        {
            _projetoRepository.AddIntegrante(idUsuario, id);

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult DeleteIntegrante(string idUsuario, int id)
        {
            _projetoRepository.DeleteIntegrante(idUsuario, id);

            return RedirectToAction("Details", new { id = id });
        }

    }
}
