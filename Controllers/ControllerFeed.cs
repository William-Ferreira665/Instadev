using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.Controllers
{
        [Route("Feed")]
    public class FeedController : Controller
    {
        Usuario UsuarioFeed = new Usuario();
        Post PostFeed = new Post();
        public IActionResult Index()
        {
            List<Usuario> lista = UsuarioFeed.ListarUsuarios();
            int id = int.Parse(HttpContext.Session.GetString("_Id"));
            Usuario Logado = lista.Find(x => x.Id == id);

            ViewBag.UsuarioLogado = Logado;
            ViewBag.Stories = UsuarioFeed.ListarUsuarios();
            ViewBag.Posts = PostFeed.ListarPosts();
            return View();
        }

        [Route("CadastrarPost")]
        public IActionResult CadastrarPost(IFormCollection form)
        {
            Post NovoPost = new Post();
            List<Post> Listagem = PostFeed.ListarPosts();
            List<int> ListaIDs = new List<int>();

            NovoPost.Texto = form["Descricao"];

            NovoPost.IdAutor = int.Parse(HttpContext.Session.GetString("_Id"));
            NovoPost.NomeAutor = HttpContext.Session.GetString("Nome");
            NovoPost.ImagemAutor = HttpContext.Session.GetString("Imagem");

            foreach (Post item in Listagem)
            {
                ListaIDs.Add(item.IdPost);
            }

            int IdPost;

            do
            {
                Random randNum = new Random();
                IdPost = randNum.Next(0, 9999);
            } while (ListaIDs.FindAll(x => x == IdPost) == null);

            NovoPost.IdPost = IdPost;
            if (form.Files.Count > 0)
            {

                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Posts");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                NovoPost.imagem = file.FileName;
            }

            PostFeed.CadastrarPost(NovoPost);

            return LocalRedirect("~/Feed");
        }
    }
}