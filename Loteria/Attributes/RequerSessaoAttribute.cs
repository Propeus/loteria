using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Attributes
{
    public class RequerSessaoAttribute : Attribute
    {
        public bool PossuiSessao { get; private set; }
        public string Action { get; private set; }
        public string Controller { get; private set; }

        public RequerSessaoAttribute(string action,string controller)
        {
            PossuiSessao = Helper.Helper.PossuiSessaoUsuario();
            Action = action;
            Controller = controller;
        }
    }
}