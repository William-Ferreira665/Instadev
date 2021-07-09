using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.Controllers
{
    public class ControllerCadastro
    {
        [Route("Cadastro")]
    public class CadastroController : Controller
    {
        Usuario UsuarioModel = new Usuario();
        public IActionResult Index()
        {
            ViewBag.Usuarios = UsuarioModel.ListarUsuarios();
            return View();
        }

        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Usuario NovoUsuario = new Usuario();
            NovoUsuario.Nome = form["NomeCompleto"];
            NovoUsuario.Email = form["E-Mail"];
            NovoUsuario.SetarSenha(form["Senha"]);
            NovoUsuario.NomeUsuario = (form["NomeUsuario"]);

            List<Usuario> Listagem = UsuarioModel.ListarUsuarios();
            List<int> ListaIDs = new List<int>();

            foreach (Usuario item in Listagem)
            {
                ListaIDs.Add(item.Id);
            }
            
            int idUser;

            do
            {
                Random randNum = new Random();
                idUser = randNum.Next(0, 9999);
            } while (ListaIDs.FindAll(x => x == idUser) == null);

            NovoUsuario.Id = idUser;

            NovoUsuario.ImagemPerfil = "padrao.png";

            UsuarioModel.CadastrarUsuario(NovoUsuario);
            ViewBag.Usuarios = UsuarioModel.ListarUsuarios();

            return LocalRedirect("~/Cadastro");
        }
    }
}