using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.Controllers
{
        [Route("Perfil")]
    public class PerfilController : Controller
    {

        Post publicacao = new Post();
        Usuario user = new Usuario();

        public IActionResult Index()
        {
            int id = int.Parse(HttpContext.Session.GetString("_Id"));
            List<Post> publi = publicacao.ListarPosts();
            List<Post> postagens = publi.FindAll(x => x.IdAutor == id);

            List<Usuario> users = user.ListarUsuarios();
            Usuario usuarioLogado = users.Find(y => y.Id == id);

            ViewBag.Posts = postagens;
            ViewBag.Perfil = usuarioLogado;
            
            return View();
        }

        [Route("Deslogar")]
        public IActionResult Deslogar(){
            HttpContext.Session.Remove("_Id");
            HttpContext.Session.Remove("Nome");
            HttpContext.Session.Remove("Imagem");
            return LocalRedirect("~/Login");
        }
    }
}