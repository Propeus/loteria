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

                    using (sorteioService = new SorteioService())
                    {
                        using (UsuarioService = new UsuarioService(sorteioService.Repository.RepositoryFactory))
                        {
                            model.Sorteio.DataSorteio = DateTime.Now;
                            model.Sorteio.Usuarios = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
                            sorteioService.InserirSorteio(model.Sorteio);
                        }
                    }
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

        [RequerSessao("Inicio", "Painel")]
        public ActionResult Cadastrar()
        {

            InicializarModel(sorteioViewModel);
            return View(sorteioViewModel);
        }

        public ActionResult Visualizar()
        {
            using (sorteioService = new SorteioService())
            {
                sorteioViewModel.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
            }
            sorteioViewModel.Usuario = Session["User"] as Usuarios ?? new Usuarios();
            return View(sorteioViewModel);
        }

        [HttpPost]
        public ActionResult Pesquisar(PesquisaViewModel model)
        {
            using (sorteioService = new SorteioService())
            {

                if (model.Ano != 0 && model.Mes != 0)
                {
                    sorteioViewModel.Sorteios = sorteioService.RecuperarSorteiosPorMesAno(model.Mes, model.Ano);
                }
                else if (model.Ano != 0)
                {
                    sorteioViewModel.Sorteios = sorteioService.RecuperarSorteiosPorAno(model.Ano);
                }
                else if (model.Mes != 0)
                {
                    sorteioViewModel.Sorteios = sorteioService.RecuperarSorteiosPorMes(model.Mes);
                }
                else
                {
                    sorteioViewModel.Sorteios = sorteioService.RecuperarSorteiosPorAno(DateTime.Now.Year);

                }
            }
            sorteioViewModel.Usuario = (Session["User"] as Usuarios) ?? new Usuarios();
            return View("Visualizar", sorteioViewModel);
        }

        private void InicializarModel(SorteioViewModel model)
        {
            using (UsuarioService = new UsuarioService())
            {
                using (sorteioService = new SorteioService(UsuarioService.Repository.RepositoryFactory))
                {
                    model.Usuario = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
                    model.Sorteios = sorteioService.Repository.RecuperarPorAno(DateTime.Now.Year).ToList();
                }
            }
        }
    }
}