using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Helper
{
    public static class Helper
    {
        public static bool PossuiSessaoUsuario()
        {
            return HttpContext.Current.Session["User"] != null;
        }

        public static Repository.IdcTipoConta UsuarioSessaoTipoConta()
        {
            var usuario = HttpContext.Current.Session["User"];
             if (usuario!= null)
            {
                return (usuario as Usuarios).TipoConta;
            }
            else
            {
                return IdcTipoConta.Usuario;
            }
        }
    }
}