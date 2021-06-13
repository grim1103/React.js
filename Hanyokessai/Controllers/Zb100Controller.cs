using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;

namespace Hanyokessai.Controllers
{
    public class Zb100Controller : Controller
    {

        /// ユーザ情報管理一覧画面に転移する。
        public IActionResult event_Initial()
        {
            string com = HttpContext.Session.GetString("companyCd");
            string auth = HttpContext.Session.GetString("authorityCd");
            
            
            //DB Connect
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<Zb100Dto> zb100 = context.GetUserInfoByMailZb100(com);
            if(auth == "0") {
                zb100.Clear();
            }

                return View("Zb100", zb100);
        }

        public IActionResult event_Cancel()
        {
            
            return RedirectToAction("event_initial", "Za100");
        }

        public IActionResult event_Inquiry(string Mail)
        {
            TempData["Mail"] = Mail;

            return RedirectToAction("event_initial", "Zb200");
        }

        [HttpPost]
        public IActionResult event_Delete(string Mail)
        {
            string com = HttpContext.Session.GetString("companyCd");
            string userId = HttpContext.Session.GetString("memberId");
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            if (Mail != null) {
                //DB Connect
                context.UpdateUserInfoByMail(Mail, userId);
            }

            List<Zb100Dto> zb100 = context.GetUserInfoByMailZb100(com);

            return View("zb100", zb100);
        }
    }
}
