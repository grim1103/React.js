using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Hanyokessai.Controllers
{
    public class ZA600Controller : Controller
    {
        /// <summary>
        /// 会員情報照会
        /// 内容：セッション情報から取得したユーザーメールを基にして会員情報を画面に表示する。
        /// </summary>
        /// <returns>自画面遷移</returns>
        public IActionResult event_Initial()
        {
            string email = HttpContext.Session.GetString("mailAddr");
            //DB Connect
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za600Dto za600 = new Za600Dto();
            za600 = context.GetUserInfoByMailZa600(email);

            if (za600.labelStamp == "1") {
                //イメージデータをURL形式に変更
                za600.imgStamp = "data: image / png; base64," + za600.imgStamp;
            }
            return View("Za600", za600);
        }

        /// <summary>
        /// 戻るボタンを押下
        /// 内容：ZA100_メイン画面に遷移する。
        /// </summary>
        /// <returns>Main Page</returns>
        /// 
        public IActionResult event_Cancel()
        {

            return RedirectToAction("event_initial", "Za100");
        }

        /// <summary>
        /// 会員情報修正ボタンを押下
        /// 内容：ZA400_会員情報修正画面に遷移する。
        /// </summary>
        /// <returns>Member Modify page</returns>

        public IActionResult event_Modify()
        {

            return RedirectToAction("event_Initial", "Za400");
        }

        /// <summary>
        /// 会員脱会ボタンを押下
        /// 内容：ZA500_会員脱会画面に遷移する。
        /// </summary>
        /// <returns>Unregister Page</returns>

        public IActionResult event_UnRegister()
        {
            HttpContext.Session.SetString("gamenID", "Za600");
            return RedirectToAction("event_Initial", "Za500");
        }
    }
}

