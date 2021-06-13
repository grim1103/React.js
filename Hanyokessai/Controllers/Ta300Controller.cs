using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hanyokessai.Controllers
{
    public class Ta300Controller : Controller
    {
        /// テンプレート一覧画面に転移する。
        public IActionResult event_Initial()
        {
            string templeteId = null;
            if (TempData.ContainsKey("TempleteId"))
            {
                templeteId = TempData["TempleteId"].ToString();
            }

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ta300Dto ta300 = context.SelectTempletTa300(templeteId);

            if (!Common.IsNullOrEmpty(ta300.upd_id))
            {
                ta300 = context.SelectUpdateMemberTa300(ta300);
            }

            return View(Constants.View.TA300, ta300);
        }

        public IActionResult event_Modify(string templete_id)
        {
            TempData["TempleteId"] = templete_id;
            return RedirectToAction("event_initial", Constants.View.TA400);
        }

        public IActionResult event_Delate(string templete_id)
        {
            string memberId = HttpContext.Session.GetString("memberId");
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            context.UpdateTempletTa300(memberId, templete_id);
            return RedirectToAction("event_initial", Constants.View.TA200);
        }

        public IActionResult event_Cancel()
        {

            return RedirectToAction("event_initial", Constants.View.TA200);
        }

        public IActionResult event_TmpDownload(string templete_id, string file_name, string file_binary, string templete_file_size)
        {
            UInt32 templeteFileSize = Convert.ToUInt32(templete_file_size);
            Common.templateDownload(file_name, file_binary, templeteFileSize);
            TempData["TempleteId"] = templete_id;
            return RedirectToAction("event_Initial", Constants.View.TA300);
        }
    }
}

