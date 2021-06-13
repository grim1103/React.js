using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Hanyokessai.Controllers
{
    public class Ca300Controller : Controller
    {
        
        /// <summary>
        /// default action 
        /// 内容：帳票一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns
        public IActionResult event_Initial()
        {
            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            //日付取得し、該当月の最終日を計算する
            DateTime today = DateTime.Now;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            string endDate = startDate.AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            //string todayYm = today.ToString("yyyy-MM");
            TempData["gaitoYm"] = today.ToString("yyyyMM");
            TempData["gaitoYmCheck"] = today.ToString("yyyy.MM");
            List<Ca300Dto> ca300Dto = context.SelectCa300(userId, endDate);//TODO:userId

            return View("Ca300",ca300Dto);
        }

        /// <summary>
        /// default action 
        /// 内容：前月の帳票一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns
        public IActionResult Report_Previous ()
        {
            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            //日付取得し、該当月の最終日を計算する
            string stringDate = TempData["gaitoYm"].ToString();
            string previousYear = stringDate.Substring(0, 4);
            string previousMon = stringDate.Substring(4, 2);
            DateTime preDate = new DateTime(Int32.Parse(previousYear), Int32.Parse(previousMon), 1);
            DateTime startPreDate = preDate.AddMonths(-1);
            string endPreDate = startPreDate.AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            TempData["gaitoYm"] = startPreDate.ToString("yyyyMM");
            TempData["gaitoYmCheck"] = startPreDate.ToString("yyyy.MM");
            List<Ca300Dto> ca300Dto = context.SelectCa300(userId, endPreDate);//TODO:userId

            return View("Ca300", ca300Dto);
        }

        /// <summary>
        /// default action 
        /// 内容：次月の帳票一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult Report_Next()
        {
            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            //日付取得し、該当月の最終日を計算する
            string stringDate = TempData["gaitoYm"].ToString();
            string nextYear = stringDate.Substring(0, 4);
            string nextMon = stringDate.Substring(4, 2);
            DateTime nextDate = new DateTime(Int32.Parse(nextYear), Int32.Parse(nextMon), 1);
            DateTime startNextDate = nextDate.AddMonths(1);
            string endNextDate = startNextDate.AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            TempData["gaitoYm"] = startNextDate.ToString("yyyyMM");
            TempData["gaitoYmCheck"] = startNextDate.ToString("yyyy.MM");
            List<Ca300Dto> ca300Dto = context.SelectCa300(userId, endNextDate);//TODO:userId

            return View("Ca300", ca300Dto);
        }

        /// <summary>
        /// default action 
        /// 内容：帳票作成画面に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult Report_Write()
        {

            return RedirectToAction("event_Initial", "Ca100");
        }

        /// <summary>
        /// default action 
        /// 内容：該当帳票ファイルをダウンロードする
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult Report_Download(string templeteID, string reportID, string fileName, string binaryFile, string fileSize)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            DownloadDto downloadDto = context.SelectReportInfoForDownlord(reportID);
            UInt32 templeteFileSize = Convert.ToUInt32(fileSize);
            Common.reportDownload(templeteID, downloadDto, fileName, binaryFile, templeteFileSize);
            return RedirectToAction("event_Initial", "Ca300");
        }
    }
}
