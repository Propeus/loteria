using Loteria.Attributes;
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
    public class SorteioController : GenericoController
    {
        SorteioViewModel sorteioViewModel = new SorteioViewModel();
       
        #region Serviços
        SorteioService sorteioService;
        ApostaService apostaService;
        UsuarioService UsuarioService;
        #endregion

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

        [RequerSessao("Inicio", "Painel")]
        [HttpPost]
        public ActionResult RegistrarNumeros(SorteioViewModel model)
        {
           

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

        [RequerSessao("Inicio", "Painel")]
        public ActionResult Cadastrar()
        {
            
            InicializarModel(sorteioViewModel);
            return View(sorteioViewModel);
        }

        public ActionResult Visualizar()
        {
            sorteioViewModel.Usuario = Session["User"] as Usuarios ?? new Usuarios();
            sorteioViewModel.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
            return View(sorteioViewModel);
        }

        private void InicializarModel(SorteioViewModel model)
        {
            model.Usuario = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
            model.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
        }
    }
}