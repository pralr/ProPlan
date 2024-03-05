using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManager.Models;
using ProjectManager.ViewModels;
using System.ComponentModel;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectManager.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ContaController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public struct Constantes
        {
            public const string USUARIO_INVALIDO = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
            public const string ERRO_CADASTRO = "Ocorreu um erro ao cadastrar o usuário. Por favor, tente novamente.";
            public const string SUCESSO_CADASTRO = "Usuário cadastrado com sucesso!";
            public const string SUCESSO_ATUALIZACAO = "Suas atualizações foram salvas!";
            public const string ERRO_USUARIO_CADASTRADO = "Este usuário já cadastrado.";
			public const string ERRO_SENHA = "A senha deve ter no mínimo 8 caracteres, uma letra maiúscula e um caractere especial.";
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var login = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            ViewBag.MostrarMenu = false;

            return View(login);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
           if(!ModelState.IsValid)
                return View(login);

           var user = await _userManager.FindByNameAsync(login.Usuario);

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, login.Senha, false, false);
    
                if(result.Succeeded)
                {
                     if(string.IsNullOrEmpty(login.ReturnUrl))
                          return RedirectToAction("Index", "Projeto");
                     return Redirect(login.ReturnUrl);
                }
            }

            TempData["ErroLogin"] = Constantes.USUARIO_INVALIDO;

            ModelState.AddModelError(string.Empty, Constantes.USUARIO_INVALIDO);
            return View(login);
        }

        public IActionResult Cadastro()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors);

                TempData["ErroCadastro"] = Constantes.ERRO_CADASTRO + string.Join(", ", erros.Select(e => e.ErrorMessage));

                return View(login);
            }

            var user = new ApplicationUser()
            {
                UserName = login.Usuario
            };

            var result = await _userManager.CreateAsync(user, login.Senha);

            if (result.Succeeded)
            {
                TempData["SucessoCadastro"] = Constantes.SUCESSO_CADASTRO;

                return RedirectToAction("Cadastro", "Conta");
            }

            foreach (var erro in result.Errors)
            {
                if (erro.Code == "DuplicateUserName")
                {
                    TempData["ErroCadastro"] = Constantes.ERRO_USUARIO_CADASTRADO;
				}
				else
                {
                    TempData["ErroCadastro"] = Constantes.ERRO_SENHA;
				}

                ModelState.AddModelError(string.Empty, erro.Description);
            }

            return View(login);
        }

        [HttpGet]
        public async Task<IActionResult> Profile (string id)
        {
            PopulaViewBags();

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            UsuarioViewModel model = new UsuarioViewModel
            {
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                Descricao = user.Descricao,
                Email = user.Email,
                Telefone = user.PhoneNumber,
                TipoCargo = user.TipoCargo
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            PopulaViewBags();

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            UsuarioViewModel model = new UsuarioViewModel
            {
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                Descricao = user.Descricao,
                Email = user.Email,
                Telefone = user.PhoneNumber,
                TipoCargo = user.TipoCargo,
            };

            if (TempData["SucessoAtualizacao"] != null)
                ViewBag.SucessoAtualizacao = TempData["SucessoAtualizacao"].ToString();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UsuarioViewModel model)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            user.Nome = model.Nome;
            user.Sobrenome = model.Sobrenome;
            user.Descricao = model.Descricao;
            user.Email = model.Email;
            user.PhoneNumber = model.Telefone;
            user.TipoCargo = model.TipoCargo;
            //user.Projetos = model.Projetos;
            //user.Atividades = model.Atividades;

            var result = await _userManager.UpdateAsync(user);
           

            if (result.Succeeded)
            {
                TempData["SucessoAtualizacao"] = Constantes.SUCESSO_ATUALIZACAO;
                return RedirectToAction("Edit", new { id = user.Id });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); 
            HttpContext.User = null;

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Conta");
        }


        public IActionResult AcessoNegado()
        {
            return View();
        }

        public void PopulaViewBags()
        {
            ViewBag.TipoCargo = Enum.GetValues(typeof(ProjectManager.Enums.EnumTipoCargo))
                         .Cast<ProjectManager.Enums.EnumTipoCargo>()
                         .Select(e => new SelectListItem
                         {
                             Value = ((int)e).ToString(),
                             Text = GetDescription(e)
                         })
                         .ToList();

        }

        public string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
