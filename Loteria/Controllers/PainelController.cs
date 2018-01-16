using Loteria.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loteria.Controllers
{
    public class PainelController : Controller
    {
        PainelModelViewModel painelModel = new PainelModelViewModel();
        SorteioRepository sorteioRepository;
        UsuariosRepository usuariosRepository;

        public PainelController()
        {
            sorteioRepository = new SorteioRepository();
            usuariosRepository = new UsuariosRepository(sorteioRepository.RepositoryFactory); 
        }

        // GET: Painel
        public ActionResult Inicio()
        {
            painelModel.Sorteios = sorteioRepository.RecuperarPorAno(DateTime.Now.Year).ToList();
            painelModel.Usuario = Session["User"] as Usuarios ?? new Usuarios();
            return View(painelModel);
        }


    }
}