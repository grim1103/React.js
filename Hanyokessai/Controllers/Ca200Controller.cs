using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanyokessai.Controllers
{
    public class Ca200Controller : Controller
    {
        /// <summary>
        /// default action 
        /// 内容：帳票修正画面に遷移する。
        /// </summary>
        /// <returns>帳票照会画面</returns>
        public IActionResult event_Initial(string reportId, string templeteId, bool btnFlg)
        {
            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ca200Dto ca200Dto = context.SelectCa200Dto(reportId, templeteId, userId);

            //Ca400から取得した変数を設定
            ca200Dto.reportID = reportId;
            ca200Dto.templeteID = templeteId;
            ca200Dto.rejectFlg = btnFlg;

            return View("Ca200", ca200Dto);
        }

        /// <summary>
        /// default action 
        /// 内容：帳票照会画面に遷移する。（修正）
        /// </summary>
        /// <returns>帳票照会画面</returns>
        public IActionResult Report_Modify(Ca200Dto ca200Dto, string reportID, string templeteID, IFormFile reportFile)
        {
            string text = null;
            string fileName = null;
            int fileLen = 0;

            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            string file = Common.filUpload(reportFile);


            if (reportFile != null)
            {
                if (file != null)
                {
                    ca200Dto.stampCd = "1";
                    // fileRead
                    Stream a = reportFile.OpenReadStream();

                    // Stream → byte
                    byte[] b = GetByteArrayFromStream(a);

                    // byte → base64 
                    text = Convert.ToBase64String(b);
                    fileName = Path.GetFileName(reportFile.FileName);
                    fileLen = Convert.ToInt32(reportFile.Length);
                }
                else
                {
                    ca200Dto.stampCd = "0";
                    text = "";
                    fileName = "";
                    fileLen = 0;

                }
            }
            ca200Dto.reportFile = fileName;
            ca200Dto.fileSize = fileLen;
            ca200Dto.updID = userId;
            ca200Dto.updDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ca200Dto.befReportID = reportID;
            ca200Dto.templeteID = templeteID;
            context.UpdateCa200(ca200Dto, text);

            //CA400に帳票ID、テンプレートIDを転送する
            TempData["reportIDcheck"] = reportID;
            TempData["templeteIDcheck"] = templeteID;

            HttpContext.Session.SetString("gamenID", "Ca200");
            return RedirectToAction("event_Initial", "Ca400");

        }

        /// <summary>
        /// メモリーストリームを配列に変換する
        /// 内容：メモリーストリームを配列に変換する
        /// </summary>
        /// <returns>ms.ToArray</returns>
        public static byte[] GetByteArrayFromStream(Stream sm)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                sm.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// default action 
        /// 内容：帳票照会画面に遷移する。（再登録）
        /// </summary>
        /// <returns>main page</returns>       
        public IActionResult Report_Reregister(Ca200Dto ca200Dto, string reportID, string templeteID, IFormFile reportFile)
        {
            string text = null;
            string fileName = null;
            int fileLen = 0;

            //セッション取得
            string userId = HttpContext.Session.GetString("memberId");

            //DB接続
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            string file = Common.filUpload(reportFile);

            if (reportFile != null)
            {
                if (file != null)
                {
                    ca200Dto.stampCd = "1";
                    // fileRead
                    Stream a = reportFile.OpenReadStream();

                    // Stream → byte
                    byte[] b = GetByteArrayFromStream(a);

                    // byte → base64 
                    text = Convert.ToBase64String(b);
                    fileName = Path.GetFileName(reportFile.FileName);
                    fileLen = Convert.ToInt32(reportFile.Length);
                }
                else
                {
                    ca200Dto.stampCd = "0";
                    text = "";
                    fileName = "";
                    fileLen = 0;
                }
            }
            ca200Dto.reportFile = fileName;
            ca200Dto.fileSize = fileLen;
            ca200Dto.regID = userId;
            ca200Dto.regDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string getDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            ca200Dto.delCd = "0";
            ca200Dto.befReportID = reportID;
            ca200Dto.templeteID = templeteID;
            ca200Dto.reportID = templeteID + userId + getDate;
            ca200Dto.regYm = DateTime.Now.ToString("yyyyMM");

            //Insert
            context.InsertCa200Reregister(ca200Dto, text);

            //CA400に帳票ID、テンプレートIDを転送する
            TempData["reportIDcheck"] = ca200Dto.reportID;
            TempData["templeteIDcheck"] = templeteID;

            HttpContext.Session.SetString("gamenID", "Ca200");
            return RedirectToAction("event_Initial", "Ca400");

        }

        /// <summary>
        /// default action 
        /// 内容：帳票照会画面に遷移する。（キャンセル）
        /// </summary>
        /// <returns>帳票照会画面</returns>  
        public IActionResult Report_Cancel(string reportID, string templeteID)
        {

            //CA400に帳票ID、テンプレートIDを転送する
            TempData["reportIDcheck"] = reportID;
            TempData["templeteIDcheck"] = templeteID;

            return RedirectToAction("event_Initial", "Ca400",new { reportID, templeteID });
        }

    }
}
