using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Repositories.Interfaces;
using ProjectManager.ViewModels;

namespace ProjectManager.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
           _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index(string adicao, int id)
        {
            var usuariosDoRepositorio = _usuarioRepository.Usuarios;

            var usuariosViewModel = usuariosDoRepositorio.Select(usuario => new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                TipoCargo = usuario.TipoCargo, 
                Projetos = usuario.Projetos, 
                Email = usuario.Email

            }).ToList();

            ViewBag.Adicao = adicao;
            ViewBag.Id = id;

            return View(usuariosViewModel);
        }


        public ViewResult BuscarUsuario(string id)
        {
            var usuarioRepositorio = _usuarioRepository.GetUsuarioById(id);

            var usuarioViewModel = new UsuarioViewModel
            {
                Nome = usuarioRepositorio.Nome,
                Sobrenome = usuarioRepositorio.Sobrenome,
                Descricao = usuarioRepositorio.Descricao,
                TipoCargo = usuarioRepositorio.TipoCargo, 
                Projetos = usuarioRepositorio.Projetos, 
                Atividades = usuarioRepositorio.Atividades, 
                ValorBolsa = usuarioRepositorio.ValorBolsa,
                Email = usuarioRepositorio.Email,
                Telefone = usuarioRepositorio.PhoneNumber
            };

            return View("Index", usuarioViewModel);
        }
    }
}
