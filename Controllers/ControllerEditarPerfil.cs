using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.Controllers
{
    public class ControllerEditarPerfil
    {
        
    [Route("EdicaoPerfil")]
    public class EdicaoPerfilController : Controller
    {
        Usuario UsuarioEdit = new Usuario();
        Post PostEdit = new Post();

        [Route("Testando")]
        public IActionResult Index()
        {
            List<Usuario> lista = UsuarioEdit.ListarUsuarios();
            int id = int.Parse(HttpContext.Session.GetString("_Id"));
            Usuario LogadoEdit = lista.Find(x => x.Id == id);
            ViewBag.UsuarioEditar = LogadoEdit;
            return View();
        }

        [Route("EditandoTeste")]
        public IActionResult Editando(IFormCollection form)
        {

            string caminho = "Database/Usuario.csv";
            int id = int.Parse(HttpContext.Session.GetString("_Id"));
            List<Usuario> userLines = UsuarioEdit.ListarUsuarios();
            List<string> linhas = UsuarioEdit.LerTodasLinhasCSV(caminho);

            Usuario usuarioEditado = userLines.Find(x => x.Id == id);
            string Nome = form["NomeEditado"];
            string User = form["NomeUsuarioEditado"];
            string Email = form["EmailEditado"];
            if (Nome != null)
            {
                usuarioEditado.Nome = Nome;
            }

            if (User != null)
            {
                usuarioEditado.NomeUsuario = User;
            }
            if (Email != null)
            {
                usuarioEditado.Email = Email;
            }

            if (form.Files.Count > 0)
            {
                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Usuarios");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                HttpContext.Session.SetString("Imagem", file.FileName);
                usuarioEditado.ImagemPerfil = file.FileName;

                List<string> Postagens = PostEdit.LerTodasLinhasCSV("Database/Post.csv");
                List<string> PostagensUser = Postagens.FindAll(x => x.Split(";")[6] == id.ToString());
                foreach (var item in PostagensUser)
                {
                    item.Split(";")[0] = form["NomeEditado"];
                    item.Split(";")[1] = file.FileName;
                }

                Postagens.RemoveAll(x => x.Split(";")[6] == id.ToString());
                
                foreach (var item in PostagensUser)
                {
                    Postagens.Add(item);
                }
                PostEdit.ReescreverCSV("Database/Post.csv", Postagens);
            }
            else
            {
                usuarioEditado.ImagemPerfil = "padrao.png";
            }

            UsuarioEdit.EditarUsuario(usuarioEditado);




            return LocalRedirect("~/Perfil");
        }
        public IActionResult Desativar()
        {
            int id = int.Parse(HttpContext.Session.GetString("_Id"));
            UsuarioEdit.DeletarUsuario(id);
            return LocalRedirect("~/Login");
        }
    }
}