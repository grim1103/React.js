using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Diagnostics;

namespace Hanyokessai.Controllers
{
    public class Ta400Controller : Controller
    {
        /// テンプレート一覧画面に転移する。
        public IActionResult event_initial()
        {
            Ta400Dto ta400 = new Ta400Dto();
            string auth = HttpContext.Session.GetString("authorityCd");
            //string temp_id = "t00002";
            string temp_id = TempData["TempleteId"].ToString();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            
            ta400.auth = auth;

            //DB Connect
            if (auth == "1") {
                ta400 = context.Selecttempleteta400(temp_id);
            }
            ta400.temp_id = temp_id;
            ta400.errorMessege = new List<string>();

            return View("Ta400", ta400);
        }

        public IActionResult event_main()
        {
            return RedirectToAction("event_initial", Constants.View.ZA200);

        }

        [HttpPost]
        public IActionResult event_cancel(string temp_id)
        {

            TempData["TempleteId"] = temp_id;
            return RedirectToAction("event_initial", Constants.View.TA300);
        }

        [HttpPost]
        public IActionResult event_modify(Ta400Dto ta400, IFormFile reportFile, string temp_id)
        {
            string userid = HttpContext.Session.GetString("memberId");
            string auth = HttpContext.Session.GetString("authorityCd");

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            
            ta400 = ErrorCheck(ta400, reportFile);

            //Sessionチェック
            if(userid is null)
            {
                return RedirectToAction("event_initial", Constants.View.ZA200);
            }

            //
            if (0 != ta400.errorMessege.Count)
            {
                ta400 = context.Selecttempleteta400(ta400);
                return View("Ta400", Constants.View.TA400);
            } else
            {
                //TODO：アップロード処理

                //ca100Dto.fileSize = Convert.ToInt32(info.Length);
                ta400.reportFile = reportFile.FileName;

                // fileSize格納

                ta400.fileSize = (UInt32)reportFile.Length;

                // fileRead
                Stream a = reportFile.OpenReadStream();

                // Stream → byte
                byte[] b = GetByteArrayFromStream(a);

                // byte → base64 
                ta400.file_binary = Convert.ToBase64String(b);

                ta400.txtRenewalrName = userid;
                ta400.txtRenewalrDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                context.UpdateTempleteta400(ta400, temp_id);

                TempData["TempleteId"] = ta400.txtTemId;

                return RedirectToAction("event_initial", Constants.View.TA300);
            }
        }

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

        public Ta400Dto ErrorCheck(Ta400Dto ta400, IFormFile reportFile)
        {
            ta400.errorMessege = new List<string>();

            // 押印ファイルがない場合
            if (reportFile == null)
            {
                ta400.errorMessege.Add("ファイルを登録してください。");

            }

            return ta400;
        }
    }
}
