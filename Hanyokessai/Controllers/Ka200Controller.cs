using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hanyokessai.Controllers
{
    public class Ka200Controller : Controller
    {
        /// <summary></summary>
        /// 決裁管理一覧画面に遷移する。
        public IActionResult event_Initial()
        {
            //セッションからユーザー情報を取得
            string usrSsseionId = HttpContext.Session.GetString("mailAddr");
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<Ka200Dto> Ka200 = new List<Ka200Dto>();
            //ユーザーIDが含めているメールリストを取得。
            List<string> mailList = OuinGroup(usrSsseionId);
            string sqlMailList = listHensyu(mailList);
            //メールリストをセッションに格納
            Common.SetObjectAsJson(HttpContext.Session, "mailList", mailList);

            //メールリストで該当する帳票IDを取得
            List<string> reportList = Listsyutoku(usrSsseionId, sqlMailList);
            //レポートリストがある場合以下の処理を行う
            if (reportList != null && reportList.Count > 0)
            {
                string sqlReportList = listHensyu(reportList);
                //該当する帳票リストをセッションに格納
                Common.SetObjectAsJson(HttpContext.Session, "reportList", reportList);

                //最新帳票の決裁日を基に検索対象の年月を抽出する。
                string preStartDate = nengatu(sqlReportList);
                (string startDate, string endDate) = nengatuKeisan(preStartDate, 0);

                //決裁管理一覧データを取得
                Ka200 = context.approvalSrch(usrSsseionId, sqlReportList, startDate, endDate, false, null);

                //決裁ステータスによりボタンフラグを設定
                Ka200 = statusTikan(Ka200, mailList, usrSsseionId, true);
            }
            else 
            {
                TempData["Disabled"] = "Disabled";
            }

            return View("Ka200", Ka200);
        }

        /// <summary>
        /// default action 
        /// 内容：設定された条件を掛け、決裁管理一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns
        public IActionResult event_Serch(string email, string selectBox, bool checkBox)
        {
            //email形式チェック
            if (email != null) 
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    ViewBag.Message = string.Format("Please check your email");
                    email = null;
                    selectBox = "option1";
                    checkBox = true;
                }
            }
            string usrSsseionId = HttpContext.Session.GetString("mailAddr");
            List<string> mailList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "mailList");
            List<string> reportList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "reportList");
            List<Ka200Dto> Ka200 = new List<Ka200Dto>();
            //レポートリストがある場合以下の処理を行う
            if (reportList != null && reportList.Count > 0)
            { 
                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

                string sqlReportList = listHensyu(reportList);
                string preStartDate = HttpContext.Session.GetString("startDate");
                (string startDate, string endDate) = nengatuKeisan(preStartDate, 0);
                string selectValue = null;
                bool kensakuSts = true;
                bool kessaiCheck = checkBox;

                switch (selectBox) 
                {
                    case "option1":
                        kensakuSts = false;
                        break;
                    case "option2":
                        selectValue = "AND ((ta.approval_mail1 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail2 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail3 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail4 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail5 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail6 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail7 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail8 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail9 = '" + usrSsseionId + "') " +
                            "OR (ta.approval_mail10 = '" + usrSsseionId + "')) ";
                        break;
                    case "option3":
                        selectValue = "AND ta.status = '99'";
                        break;
                    case "option4":
                        selectValue = "AND ta.status = '98'";
                        break;
                    case "option5":
                        kensakuSts = false;
                        kessaiCheck = false;
                        break;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(selectValue);

                if (email != null)
                {
                    string SQL = "AND ta.maker_mail ='" + email + "'";
                    sb.Append(SQL);
                    kensakuSts = true;
                }


                //ユーザーIDで一覧データを取得
                Ka200 = context.approvalSrch(usrSsseionId, sqlReportList, startDate, endDate, kensakuSts, sb.ToString());

                //決裁ステータスによりボタンフラグを設定
                Ka200 = statusTikan(Ka200, mailList, usrSsseionId, kessaiCheck);
            }
            return View("Ka200", Ka200);
        }

        /// <summary>
        /// default action 
        /// 内容：前月の帳票一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns
        public IActionResult Report_Previous()
        {
            string usrSsseionId = HttpContext.Session.GetString("mailAddr");
            List<string> mailList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "mailList");
            List<string> reportList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "reportList");
            List<Ka200Dto> Ka200 = new List<Ka200Dto>();
            string preStartDate = HttpContext.Session.GetString("startDate");

            //レポートリストがある場合以下の処理を行う
            if (reportList != null && reportList.Count > 0)
            {
                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                string sqlReportList = listHensyu(reportList);
                (string startDate, string endDate) = nengatuKeisan(preStartDate, -1);

                //ユーザーIDで一覧データを取得
                Ka200 = context.approvalSrch(usrSsseionId, sqlReportList, startDate, endDate, false, null);

                //決裁ステータスによりボタンフラグを設定
                Ka200 = statusTikan(Ka200, mailList, usrSsseionId, true);
            }
            return View("Ka200", Ka200);
        }

        /// <summary>
        /// default action 
        /// 内容：次月の帳票一覧画面に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult Report_Next()
        {
            string usrSsseionId = HttpContext.Session.GetString("mailAddr");
            List<string> mailList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "mailList");
            List<string> reportList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "reportList");
            string preStartDate = HttpContext.Session.GetString("startDate");
            List<Ka200Dto> Ka200 = new List<Ka200Dto>();

            //レポートリストがある場合以下の処理を行う
            if (reportList != null && reportList.Count > 0)
            {
                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                string sqlReportList = listHensyu(reportList);

                (string startDate, string endDate) = nengatuKeisan(preStartDate, 1);

                //ユーザーIDで一覧データを取得
                Ka200 = context.approvalSrch(usrSsseionId, sqlReportList, startDate, endDate, false, null);

                //決裁ステータスによりボタンフラグを設定
                Ka200 = statusTikan(Ka200, mailList, usrSsseionId, true);
            }
            return View("Ka200", Ka200);
        }

        /// <summary>
        /// default action 
        /// 内容：KA100_決裁管理＿全体に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Toukei()
        {
            List<string> reportList = Common.GetObjectFromJson<List<string>>(HttpContext.Session, "reportList");
            string sqlReportList = listHensyu(reportList);

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            (string ksmati, string kskyaka, string kskanryo) = context.SelectKensu(sqlReportList);
            TempData["ksmati"] = ksmati;
            TempData["kskyaka"] = kskyaka;
            TempData["kskanryo"] = kskanryo;
            TempData["kszumi"] = (reportList.Count - Int32.Parse(ksmati) - Int32.Parse(kskyaka) - Int32.Parse(kskanryo));

            return RedirectToAction("event_initial", "Ka100");
        }

        /// <summary>
        /// default action 
        /// 内容：帳票をダウンロードする。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_download(string file_name, string file_binary, string report_file_size, string templeteID, string reportID)
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            DownloadDto downloadDto = context.SelectReportInfoForDownlord(reportID);
            UInt32 templeteFileSize = Convert.ToUInt32(report_file_size);
            Common.reportDownload(templeteID, downloadDto, file_name, file_binary, templeteFileSize);
            return RedirectToAction("event_Initial", "Ka200");

        }


        /// <summary>
        /// 内容：決裁ステータスによりボタンフラグを設定。
        /// </summary>
        /// <returns>Ka200Dto</returns>
        public List<Ka200Dto> statusTikan(List<Ka200Dto> ka200, List<string>mailList, string usrSsseionId, bool kessaiCheck)
        {

            bool check = kessaiCheck;
            

            for (int i = 0; i < ka200.Count; i++) 
            {
                switch (ka200[i].txtApvlSts)
                {
                    case "10":
                        ka200[i].txtApvlSts = "決裁1待ち";
                        if (ka200[i].approvalMail1.Equals(usrSsseionId)|| mailList.Contains(ka200[i].approvalMail1)) 
                        {
                            ka200[i].btnFlg = true;
                        }
                        else 
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "11":
                        ka200[i].txtApvlSts = "決裁2待ち";
                        if (ka200[i].approvalMail2.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail2))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "12":
                        ka200[i].txtApvlSts = "決裁3待ち";
                        if (ka200[i].approvalMail3.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail3))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "13":
                        ka200[i].txtApvlSts = "決裁4待ち";
                        if (ka200[i].approvalMail4.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail4))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "14":
                        ka200[i].txtApvlSts = "決裁5待ち";
                        if (ka200[i].approvalMail5.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail5))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "15":
                        ka200[i].txtApvlSts = "決裁6待ち";
                        if (ka200[i].approvalMail6.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail6))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "16":
                        ka200[i].txtApvlSts = "決裁7待ち";
                        if (ka200[i].approvalMail7.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail7))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "17":
                        ka200[i].txtApvlSts = "決裁8待ち";
                        if (ka200[i].approvalMail8.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail8))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "18":
                        ka200[i].txtApvlSts = "決裁9待ち";
                        if (ka200[i].approvalMail9.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail9))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "19":
                        ka200[i].txtApvlSts = "決裁10待ち";
                        if (ka200[i].approvalMail10.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMail10))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "90":
                        ka200[i].txtApvlSts = "最終決裁待ち";
                        if (ka200[i].approvalMailLast.Equals(usrSsseionId) || mailList.Contains(ka200[i].approvalMailLast))
                        {
                            ka200[i].btnFlg = true;
                        }
                        else
                        {
                            ka200[i].btnFlg = false;
                        }
                        break;
                    case "98":
                        ka200[i].txtApvlSts = "却下";
                        ka200[i].btnFlg = false;
                        break;
                    case "99":
                        ka200[i].txtApvlSts = "完了";
                        ka200[i].btnFlg = false;                       
                        break;
                }
            }

            if (check)
            {
                List<Ka200Dto> myList = new List<Ka200Dto>();
                for (int i=0;i<ka200.Count;i++) 
                {

                    if (ka200[i].btnFlg)
                    {
                        myList.Add(ka200[i]);
                    }
                }
                return myList;
            }
            return ka200;
        }
        /// <summary>
        /// 内容：ユーザーIDが含めているメールリストを取得。
        /// </summary>
        /// <returns>メールリスト</returns>
        public List<string> OuinGroup(string usrSsseionId)
        {
                  List<string> OuinList = new List<string>();                  
                  DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                  OuinList = context.SelectMailList(usrSsseionId);

                  return OuinList;
        }

        /// <summary>
        /// 内容：リストをSQL条件に使うため、編集する。
        /// </summary>
        /// <returns></returns>
        public string listHensyu(List<string> mailList)
        {
            StringBuilder sb = new StringBuilder();
            var i = 0;
            var count = mailList.Count;
            foreach (string sql in mailList)
            {
                sb.Append(sql);
                if (++i == count) 
                {
                    return sb.ToString();
                }
                sb.Append("','");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 内容：帳票リストを取得する。
        /// </summary>
        /// <returns>帳票リスト</returns>
        public List<string> Listsyutoku(string usrSsseionId, string sqlList) 
        {
            List<string> reportID = new List<string>();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            reportID = context.SelectReportList(usrSsseionId,sqlList);
            return reportID;
        }

        /// <summary>
        /// 内容：最新帳票の決裁日を取得する。
        /// </summary>
        /// <returns>最新帳票の決裁日</returns>
        public string nengatu(string reportList) 
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string result = context.SelectNengatuList(reportList);
            return result;
        }

        /// <summary>
        /// 内容：最新帳票の決裁日をもとに対象年月を取得する。
        /// </summary>
        /// <returns>StartDate,EndDate</returns>
        public (string a, string b) nengatuKeisan(string start, int months) 
        {
            string previousYear = start.Substring(0, 4);
            string previousMon = start.Substring(4, 2);
            DateTime startPreDate;
            if (0 != months)
            {
                startPreDate = new DateTime(Int32.Parse(previousYear), Int32.Parse(previousMon), 1).AddMonths(months);
            }
            else 
            {
                 startPreDate = new DateTime(Int32.Parse(previousYear), Int32.Parse(previousMon), 1);
            }
            DateTime endPreDate = startPreDate.AddMonths(1).AddSeconds(-1);

            string startDate = startPreDate.ToString("yyyyMMddHHmmss");
            string endDate = endPreDate.ToString("yyyyMMddHHmmss");

            TempData["gaitoY"] = startDate.Substring(0, 4);
            TempData["gaitoM"] = endDate.Substring(4, 2);

            HttpContext.Session.SetString("startDate", startDate);

            return (startDate, endDate);
        }


    }
}