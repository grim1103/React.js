using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Hanyokessai.Models;

namespace Hanyokessai.Controllers
{
    public class Za500Controller : Controller
    {
        /// <summary>
        /// default action 
        /// 内容：初期表示画面（Index　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Initial()
        {
            //初期設定
            Za500Dto za500 = new Za500Dto();
            za500.email = HttpContext.Session.GetString("mailAddr");

            return View("Za500", za500);
        }

        /// <summary>
        /// unRegister action 
        /// 内容：会員脱会処理後、ログインページに遷移する。
        /// </summary>
        /// <returns>login page</returns>
        public IActionResult event_UnRegister(Za500Dto za500)
        {
                   //単項目チェック
            if (!ModelState.IsValid)
            {
                
                return View("Za500", za500);
            }
                  /// 入力したパスワードをSHA256暗号化する
            string password = Encrypt(za500.password);
                  /// emailでDBからユーザーパスワードを取得
            string userPassword = SelectPassword(za500.email);
            /// passwordチェック
            if (!password.Equals(userPassword))
            {
                        //パスワードが一致しない場合
                ViewBag.Message = string.Format("Please check your password");
                return View("Za500", za500);

            }
                  //パスワードが一致する場合
            UpdateUserInfoByMail(za500.email);
            HttpContext.Session.Clear();
            return RedirectToAction("event_Initial", "Za200");
        }

        /// <summary>
        /// cancel action 
        /// 内容：遷移元画面に遷移する。
        /// </summary>
        /// <returns>Back page</returns>
        public IActionResult event_Cancel()
        {
            string gamenID = HttpContext.Session.GetString("gamenID");
            return RedirectToAction("event_Initial", gamenID);

        }
        /// <summary>
        /// パスワードをSHA256暗号化する
        /// 内容：passwordをSHA256変換する
        /// </summary>
        /// <returns>password</returns>
        public String Encrypt(string password)
        {
            try
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                SHA256Managed sha256Managed = new SHA256Managed();
                byte[] encryptBytes = sha256Managed.ComputeHash(passwordBytes);

                string encryptString = Convert.ToBase64String(encryptBytes);

                return encryptString;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// 内容：ユーザー情報（email）でDBからパスワードを取得する。
        /// </summary>
        /// <returns>password</returns>
        [HttpPost]
        public String SelectPassword(string email)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            string password = context.SelectPassword(email);
            return password;

        }

        /// <summary>
        /// 内容：ユーザー情報を削除する。（会員情報は物理削除し、レコードは論理削除）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void UpdateUserInfoByMail(string email)
        {

            string userId = HttpContext.Session.GetString("memberId");
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            context.UpdateUserInfoByMail(email, userId);

        }

    }
}