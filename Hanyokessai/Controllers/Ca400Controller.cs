using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using System.Security.Policy;
using Renci.SshNet.Messages;
using System;
using Microsoft.AspNetCore.Http;

namespace Hanyokessai.Controllers
{

    public class Ca400Controller : Controller
    {
        /// 帳票照会画面に転移する。
        public IActionResult Event_Initial(string reportID, string templeteID)
        {
            Ca400Dto ca400 = new Ca400Dto();
            string gamenID = HttpContext.Session.GetString("gamenID");
            string repID = null;
            string tempID = null;

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            if (gamenID == null)
            {
                gamenID = "";
            }
                  //修正画面からテンプレートIDとレポートIDを持って帳票内容を取得
            if ("Ca200".Equals(gamenID))
            {
                repID = TempData.Peek("reportIDcheck").ToString();
                tempID = TempData.Peek("templeteIDcheck").ToString();
            }
                  //一覧画面で選択した帳票内容を取得
            else
            {
                repID = reportID;
                tempID = templeteID;
            }

            ca400 = context.GetInfoBydataCa400(tempID, repID);
            return View("Ca400", ca400);
        }

        public IActionResult reportBef(string bef_report_id)
        {
            //画面表示に必要なデータを取得する。
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            string templeteid = SelectTemplete(bef_report_id);

            Ca400Dto ca400 = context.GetInfoBydataCa400(templeteid, bef_report_id);
            return View("Ca400", ca400);
        }

        [HttpPost]
        public string SelectTemplete(string report)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string templete = context.GetTempleteIDCa400(report);
            return templete;

        }

        public IActionResult reportDelete(string reportId, string templeteId)
        {

                  //画面表示に必要なデータを取得する。
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string Userid = HttpContext.Session.GetString("memberId");
            string sysDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            context.DeleteReportCa400(templeteId, reportId, Userid, sysDate);

            return RedirectToAction("event_Initial", "Ca300");
        }

        public IActionResult reportBack()
        {
            return RedirectToAction("event_Initial", "Ca300");
        }

    }
}
