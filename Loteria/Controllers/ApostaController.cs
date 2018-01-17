﻿using Loteria.Helper;
using Loteria.Models;
using Loteria.Attributes;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loteria.Controllers
{
    public class ApostaController : GenericoController
    {

        ApostasViewModel apostasViewModel = new ApostasViewModel();

        #region Serviços
        SorteioService sorteioService;
        ApostaService apostaService;
        UsuarioService UsuarioService;
        #endregion

        public ApostaController()
        {

            sorteioService = new SorteioService();
            apostaService = new ApostaService(sorteioService.Repository.RepositoryFactory);
            UsuarioService = new UsuarioService(sorteioService.Repository.RepositoryFactory);
        }

        [RequerSessao("Inicio", "Painel")]
        [HttpPost]
        public JsonResult GerarNumeros()
        {
            var Numeros = Service.Helper.Helper.SortearValores(6, 0, 60);
            return Json(new { Numeros });
        }

        [RequerSessao("Inicio", "Painel")]
        [HttpPost]
        public ActionResult RegistrarNumeros(ApostasViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model.Aposta.DataAposta = DateTime.Now;
                    model.Aposta.Usuarios = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
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

        [RequerSessao("Inicio", "Painel")]
        public ActionResult Cadastrar()
        {
            InicializarModel(apostasViewModel);
            return View(apostasViewModel);
        }

        [RequerSessao("Inicio", "Painel")]
        public ActionResult Visualizar()
        {

            InicializarModel(apostasViewModel);
            return View(apostasViewModel);
        }

        [RequerSessao("Inicio", "Painel")]
        [HttpPost]
        public ActionResult Pesquisar(PesquisaViewModel model)
        {
            InicializarModel(apostasViewModel);
            if (model.Ano != 0 && model.Mes != 0)
            {
                apostasViewModel.Apostas = apostaService.RecuperarResultadosPorMesAno(apostasViewModel.Usuario, model.Mes, model.Ano);
            }
            else if (model.Ano != 0)
            {
                apostasViewModel.Apostas = apostaService.RecuperarResultadosPorAno(apostasViewModel.Usuario, model.Ano);
            }
            else if (model.Mes != 0)
            {
                apostasViewModel.Apostas = apostaService.RecuperarResultadosPorMes(apostasViewModel.Usuario, model.Mes);
            }
            else if (model.Acertos != 0)
            {
                apostasViewModel.Apostas = apostaService.RecuperarResultadosPorAcertos(apostasViewModel.Usuario, model.Acertos);
            }
            return View("Visualizar", apostasViewModel);
        }

        private void InicializarModel(ApostasViewModel model)
        {
            model.Usuario = UsuarioService.Repository.RecuperarPorId((Session["User"] as Usuarios).Id);
            model.Apostas = apostaService.RecuperarResultadosPorAno(model.Usuario, DateTime.Now.Year).ToList();
        }


    }
}