using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loteria.Attributes;
namespace Loteria.Controllers
{
    public class GenericoController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequerSessaoAttribute sessao = (filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequerSessaoAttribute), false)).Length == 0 ? null : (filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequerSessaoAttribute), false)[0] as RequerSessaoAttribute);
            if (sessao != null && !sessao.PossuiSessao)
            {
                filterContext.Result = RedirectToAction(sessao.Action, sessao.Controller);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            TempData["Erro"] = filterContext.Exception.Message;
            filterContext.Result = RedirectToAction("Inicio", "Painel");
            base.OnException(filterContext);
        }
    }
}