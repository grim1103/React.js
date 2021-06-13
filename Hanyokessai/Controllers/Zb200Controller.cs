using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Hanyokessai.Controllers
{
    public class Zb200Controller : Controller
    {
        public IActionResult event_Initial()
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string memberId = null;
            if (TempData.ContainsKey("Mail")) 
            {
                memberId = TempData["Mail"].ToString();
            }
            Zb200Dto zb200 = context.GetUserInfoByMailZb200(memberId);
            if ("1".Equals(zb200.labelOuInUm))
            {
                zb200.labelOuInZyoHo = "data: image / png; base64," + zb200.labelOuInZyoHo;
            }
            return View("Zb200", zb200);
        }

        public IActionResult event_return()
        {
            return RedirectToAction("event_Initial", "Zb100");
        }

        public IActionResult event_modify(string kaiinMail)
        {
            return RedirectToAction("event_Initial", "Zb300",new { kaiinMail }) ;
        }


    }
}
