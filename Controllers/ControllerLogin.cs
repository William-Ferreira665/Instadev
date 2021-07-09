using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projeto.Models;

namespace projeto.Controllers
{
    public class ControllerLogin
    {
        [TempData]
        public string Mensagem { get; set; }
        Usuario LoginUsuario = new Usuario();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Logar")]
        public IActionResult Logar(IFormCollection form)
        {
            List<string> lista = LoginUsuario.LerTodasLinhasCSV("Database/Usuario.csv");
            string logado = lista.Find(x => x.Split(";")[1] == form["Email"] && x.Split(";")[2] == form["Senha"]);

            if (logado != null)
            {
                HttpContext.Session.SetString("_Id", logado.Split(";")[4]);
                HttpContext.Session.SetString("Nome", logado.Split(";")[3]);
                HttpContext.Session.SetString("Imagem", logado.Split(";")[5]);
                return LocalRedirect("~/Feed");
            }
            Mensagem = "Dados incorretos, tente novamente...";
            return LocalRedirect("~/");
        }

        private IActionResult LocalRedirect(string v)
        {
            throw new NotImplementedException();
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_Username");
            return LocalRedirect("~/");
        }
    }
}