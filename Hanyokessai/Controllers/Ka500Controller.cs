using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System;
using Org.BouncyCastle.Crypto.Tls;

namespace Hanyokessai.Controllers
{
    public class Ka500Controller : Controller
    {

        /// 決裁対象照会画面を表示する。
        public IActionResult event_Initial(string reportId, string templetId, bool btnFlg)
        {
                  //画面表示に必要なデータを取得する。
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ka500Dto ka500 = context.GetInfoBydataKa500(templetId, reportId, btnFlg);

            return View("Ka500", ka500);

        }

        //却下ボタンを押下
        public IActionResult approval_reject(string reportId, string templetId, string TXT_reject_detail)
        {      
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            //何番目に情報を入れるのかを確認
            int status = Convert.ToInt32(Selectstatus(reportId, templetId));
            string num = "";
            switch (status)
            {
                case 10:
                    num = "1";
                    break;
                case 11:
                    num = "2";
                    break;
                case 12:
                    num = "3";
                    break;
                case 13:
                    num = "4";
                    break;
                case 14:
                    num = "5";
                    break;
                case 15:
                    num = "6";
                    break;
                case 16:
                    num = "7";
                    break;
                case 17:
                    num = "8";
                    break;
                case 18:
                    num = "9";
                    break;
                case 19:
                    num = "10";
                    break;
                case 90:
                    num = "_last";
                    break;
            }

                  //ログインしているアカウントのメール情報
            string Usermail = HttpContext.Session.GetString("mailAddr");
            //ログインしているアカウントのID情報
            string Userid = HttpContext.Session.GetString("memberId");
                  //データを入れる時の日付
            string sysDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            context.ImprintApprovalKa500(TXT_reject_detail, num, reportId, Usermail, Userid, sysDate);
            return RedirectToAction("event_Initial", "Ka200");
        }

            //決裁ステータス取得
        [HttpPost]
        public String Selectstatus(string report, string templete)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string status = context.GetApprovalMailKa500(templete, report);
            return status;
        }

        //押印ボタンを押下
        public IActionResult approval_sign(string reportId, string templetId)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            //何番目に情報を入れるのかを確認
            int status = Convert.ToInt32(Selectstatus(reportId, templetId));
            string num = "";
            switch (status)
            {
                case 10:
                    status = 11;
                    num = "1";
                    break;
                case 11:
                    status = 12;
                    num = "2";
                    break;
                case 12:
                    status = 13;
                    num = "3";
                    break;
                case 13:
                    status = 14;
                    num = "4";
                    break;
                case 14:
                    status = 15;
                    num = "5";
                    break;
                case 15:
                    status = 16;
                    num = "6";
                    break;
                case 16:
                    status = 17;
                    num = "7";
                    break;
                case 17:
                    status = 18;
                    num = "8";
                    break;
                case 18:
                    status = 19;
                    num = "9";
                    break;
                case 19:
                    status = 90;
                    num = "10";
                    break;
                case 90:
                    status = 99;
                    num = "_last";
                    break;
            }

                  //ログインしているアカウントのメール情報
            string Usermail = HttpContext.Session.GetString("mailAddr");
                  //データを入れる時の日付
            string sysDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            context.InputApprovalKa500(Usermail, sysDate, num, status, reportId);

            return RedirectToAction("event_Initial", "Ka200");
        }

        //キャンセルボタンを押下
        public IActionResult approval_cancel()
        {
            return RedirectToAction("event_Initial", "Ka200");

        }
    }
}
