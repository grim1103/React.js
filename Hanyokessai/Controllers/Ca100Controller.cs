using System;
using System.Collections.Generic;
using System.IO;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Hanyokessai.Controllers
{

    public class Ca100Controller : Controller
    {

        public IActionResult event_Initial()
        {
            //セッション取得
            string userID = HttpContext.Session.GetString("memberId");

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ca100Dto ca100Dto = context.SelectCa100MemberDto(userID);
            List<Ca100SubDto> ca100SelectoLists = context.SelectCa100DtoList(ca100Dto.lbComName);
            ViewBag.data = ca100SelectoLists;

            ca100Dto.errorMessege = new List<string>();

            return View("Ca100", ca100Dto);
        }

        [HttpGet]
        public ActionResult event_Select(string templeteID)
        {
            string userID = HttpContext.Session.GetString("memberId");
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ca100Dto ca100Dto = context.SelectCa100DtoApprove(userID, templeteID);

            return PartialView("Ca100partialView", ca100Dto);
        }

        public IActionResult event_Register(Ca100Dto ca100Dto, IFormFile reportFile)
        {
            //セッション取得
            string mail = HttpContext.Session.GetString("mailAddr");
            string userID = HttpContext.Session.GetString("memberId");

            //日付
            string stringDate = ca100Dto.lbPlanYmd;
            string planYear = stringDate.Substring(0, 4);
            string planMon = stringDate.Substring(5, 2);
            DateTime startPlanDate = new DateTime(Int32.Parse(planYear), Int32.Parse(planMon), 1);
            string endPlanDate = startPlanDate.AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            ca100Dto = context.SelectCa100StartStampImg(ca100Dto, mail);
            if (ca100Dto.stampCd == "0")
            {
                ca100Dto.startStampImg = null;
            }

            //TODO：アップロード処理

            //ca100Dto.fileSize = Convert.ToInt32(info.Length);
            ca100Dto.reportFile = reportFile.FileName;

            // fileSize格納
            ca100Dto.fileSize = (UInt32)reportFile.Length;

            // fileRead
            Stream a = reportFile.OpenReadStream();

            // Stream → byte
            byte[] b = GetByteArrayFromStream(a);

            // byte → base64 
            ca100Dto.binaryFile = Convert.ToBase64String(b);
            ca100Dto.startAprvMail = mail;
            ca100Dto.txtPlanYmd = endPlanDate;
            ca100Dto.regID = userID;
            ca100Dto.regDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            ca100Dto.regYm = DateTime.Now.ToString("yyyyMM");
            ca100Dto.reportID = ca100Dto.selTempleteID + userID + ca100Dto.regDate;
            ca100Dto.regDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ca100Dto.delCd = "0";
            ca100Dto.status = "10";
            context.InsertCa100Register(ca100Dto);
            return RedirectToAction("event_Initial", "Ca300");
        }

        /// <summary>
        /// 所属,職位を取得する 
        /// 内容：会社名,所属,職位を取得する。
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        public Ca100Dto ca100Restart(Ca100Dto ca100Dto)
        {
            //セッション取得
            string userID = HttpContext.Session.GetString("memberId");

            Ca100Dto ca100Dto1 = new Ca100Dto();

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            ca100Dto1 = context.SelectCa100MemberDto(userID);
            List<Ca100SubDto> ca100SelectoLists = context.SelectCa100DtoList(ca100Dto1.lbComName);
            ViewBag.data = ca100SelectoLists;

            ca100Dto1.errorMessege = new List<string>();

            return ca100Dto1;
        }

        [HttpPost]
        public IActionResult event_Modify(Ca100Dto ca100Dto, IFormFile reportFile)
        {
                     // チェック処理がfalseの場合
            if (!ModelState.IsValid)
            {
                ca100Dto.errorMessege = new List<string>();
                
                ca100Dto = ca100Restart(ca100Dto);

                return View("Ca100", ca100Dto);
            }
            return event_Register(ca100Dto, reportFile);
        }

        /// <summary>
        /// default action 
        /// 内容：帳票一覧画面に遷移する。（キャンセル）
        /// </summary>
        /// <returns>帳票一覧画面</returns>
        public IActionResult event_Cancel()
        {
            return RedirectToAction("event_Initial", "Ca300");
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
    }
}
