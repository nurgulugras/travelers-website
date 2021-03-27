using Travelers.ModalLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travelers.Infrastructure.Concrete
{
    public class SessionHelper
    {
        public static LoginUser CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["login"] as LoginUser;
            }
        }
    }
}