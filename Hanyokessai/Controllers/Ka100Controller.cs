using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using System;

namespace Hanyokessai.Controllers
{
    public class Ka100Controller : Controller
    {
        /// <summary>
        /// 決裁管理＿全体画面に遷移する。
        /// </summary>
        public IActionResult event_Initial()
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<Ka100Dto> Ka100 = context.approvalReportSrch();

            Ka100Dto outputInfo = new Ka100Dto();
            outputInfo.approvalWaitNum = TempData["ksmati"].ToString();
            outputInfo.approvedNum = TempData["kszumi"].ToString();
            outputInfo.approvalRejectNum = TempData["kskyaka"].ToString();
            outputInfo.approvalComplMonNum = Ka100.Count.ToString();
            outputInfo.approvalComplTotNum = TempData["kskanryo"].ToString();
            return View("Ka100", outputInfo);
        }
    }
}
