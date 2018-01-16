using Loteria.Helper;
using Loteria.Models;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loteria.Controllers
{
    public class SorteioController : Controller
    {
        SorteioViewModel sorteioViewModel = new SorteioViewModel();
        SorteioService sorteioService;
        ApostaService apostaService;
        UsuarioService UsuarioService;
        public SorteioController()
        {
            sorteioService = new SorteioService();
            apostaService = new ApostaService(sorteioService.Repository.RepositoryFactory);
            UsuarioService = new UsuarioService(sorteioService.Repository.RepositoryFactory);
        }

        [HttpPost]
        public JsonResult GerarNumeros()
        {
            var Numeros = Service.Helper.Helper.SortearValores(6, 0, 60);
            return Json(new { Numeros });
        }

        [HttpPost]
        public ActionResult RegistrarNumeros(SorteioViewModel model)
        {
            if (!Helper.Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Inicio", "Painel");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    model.Sorteio.DataSorteio = DateTime.Now;
                    model.Sorteio.Usuarios = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
                    sorteioService.RegistrarSorteio(model.Sorteio);
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
            return View("Cadastrar",model);
        }

        public ActionResult Cadastrar()
        {
            if (!Helper.Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Inicio", "Painel");
            }
            InicializarModel(sorteioViewModel);
            return View(sorteioViewModel);
        }

        private void InicializarModel(SorteioViewModel model)
        {
            model.Usuario = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
            model.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
        }

        public ActionResult Visualizar()
        {

            sorteioViewModel.Usuario = Session["User"] as Usuarios ?? new Usuarios();
            sorteioViewModel.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
            return View(sorteioViewModel);
        }
    }
}