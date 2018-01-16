using Loteria.Helper;
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
    public class ApostaController : Controller
    {

        ApostasViewModel apostasViewModel = new ApostasViewModel();
        SorteioService sorteioService;
        ApostaService apostaService;
        UsuarioService UsuarioService;
        public ApostaController()
        {
            sorteioService = new SorteioService();
            apostaService = new ApostaService(sorteioService.sorteioRepository.RepositoryFactory);
            UsuarioService = new UsuarioService(sorteioService.sorteioRepository.RepositoryFactory);
        }

        [HttpPost]
        public JsonResult GerarNumeros()
        {
            var Numeros = Service.Helper.Helper.SortearValores(6, 0, 60);
            return Json(new { Numeros });
        }

        [HttpPost]
        public ActionResult RegistrarNumeros(ApostasViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model.Aposta.DataAposta = DateTime.Now;
                    model.Aposta.Usuarios = UsuarioService.usuarioRepository.RecuperarPorId((Session["User"] as Usuarios).Id);
                    apostaService.InserirAposta(model.Aposta);
                }
            }
            catch (AggregateException ex)
            {
                TempData["Erro"] = ex.InnerExceptions.Select(x => x.Message);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            InicializarModel(model);
            return View("Cadastrar", model);
        }

        public ActionResult Cadastrar()
        {
            if (!Helper.Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Inicio", "Painel");
            }
            InicializarModel(apostasViewModel);
            return View(apostasViewModel);
        }
        public ActionResult Visualizar()
        {
            if (!Helper.Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Inicio", "Painel");
            }

            apostasViewModel.Usuario = Session["User"] as Usuarios;
            apostasViewModel.Apostas = apostaService.RecuperarResultadosPorAno(apostasViewModel.Usuario, DateTime.Now.Year).ToList();
            return View(apostasViewModel);
        }

        private void InicializarModel(ApostasViewModel model)
        {
            model.Usuario = UsuarioService.usuarioRepository.RecuperarPorId((Session["User"] as Usuarios).Id);
            model.Apostas = apostaService.RecuperarResultadosPorAno(model.Usuario, DateTime.Now.Year).ToList();
        }


    }
}