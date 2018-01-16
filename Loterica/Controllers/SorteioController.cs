using Loteria.Helper;
using Loteria.Models;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loterica.Controllers
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
        public ActionResult RegistrarNumeros(SorteioViewModel model)
        {
            if (!Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Sorteios", "Painel");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    model.Sorteio.DataSorteio = DateTime.Now;
                    model.Sorteio.Usuarios = UsuarioService.usuarioRepository.RecuperarPorId((Session["User"] as Usuarios).Id);
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
            if (!Helper.PossuiSessaoUsuario())
            {
                return RedirectToAction("Sorteios", "Painel");
            }
            InicializarModel(sorteioViewModel);
            return View(sorteioViewModel);
        }

        private void InicializarModel(SorteioViewModel model)
        {
            model.Usuario = UsuarioService.usuarioRepository.RecuperarPorId((Session["User"] as Usuarios).Id);
            model.Sorteios = sorteioService.sorteioRepository.RecuperarPorAno(DateTime.Now.Year).ToList();
        }

        public ActionResult Visualizar()
        {

            sorteioViewModel.Usuario = Session["User"] as Usuarios ?? new Usuarios();
            sorteioViewModel.Sorteios = sorteioService.sorteioRepository.RecuperarPorAno(DateTime.Now.Year).ToList();
            return View(sorteioViewModel);
        }
    }
}