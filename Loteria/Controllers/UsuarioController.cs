using Loteria.Models;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loteria.Controllers
{
    public class UsuarioController : Controller
    {

        PainelModelViewModel painelModel = new PainelModelViewModel();
        SorteioRepository sorteioRepository;
        UsuarioService usuarioService;

        public UsuarioController()
        {
            sorteioRepository = new SorteioRepository();
            usuarioService = new UsuarioService(sorteioRepository.RepositoryFactory);
        }
  
        [HttpPost]
        public JsonResult Login(Usuarios usuarios)
        {
            if (ModelState.IsValidField("Usuario") && ModelState.IsValidField("Senha"))
            {
                var user = usuarioService.Login(usuarios.Usuario, usuarios.Senha);
                Session["User"] = user;
                if (user != null)
                    return Json(new { Sucesso = true });
            }
            return Json(new { Sucesso = false, Mensagens= new List<string> { "Login e/ou senha incoreto(s)." } });
        }

    
        [HttpPost]
        public JsonResult Registro(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                var user = usuarioService.Registrar(usuarios.Usuario, usuarios.Senha, usuarios.Detalhes.E_Mail);
                Session["User"] = user;
                if (user != null)
                    return Json(new { Sucesso = true });
            }
            return Json(new { Sucesso = false, Mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Inicio", "Painel");
        }
    }
}