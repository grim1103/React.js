using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Hanyokessai.Controllers
{

    public class ZA200Controller : Controller
    {
        LoginInfoDto loginInfoDto;  //ログイン情報Dto

        /// <summary>
        /// default action 
        /// 内容：ログイン画面（Login　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Initial()
        {
            LoginInfoDto loginInfoDto = new LoginInfoDto();
            loginInfoDto.errorMessege = new List<string>();
            // ログインチェックフラグ設定
            //loginInfoDto.txtLoginCheck = false;

            //　セッションクリアー
            HttpContext.Session.Clear();

            return View("Za200", loginInfoDto);
        }

        /// <summary>
        /// default action 
        /// 内容：会員加入画面に遷移する。
        /// </summary>
        /// <returns>会員加入 page</returns>
        public IActionResult event_Register()
        {
            return RedirectToAction("event_Initial", "Za300");
        }

        /// <summary>
        /// Loginボータンを押下
        /// 内容：email,passwordが一致する場合 Mainページに遷移する
        ///     　一致しない場合　Login　pageに遷移する
        /// </summary>
        /// <returns>Main page　or Login</returns>
        public IActionResult event_Login(LoginInfoDto loginInfoDto)
        {

            // 入力チェック
            if (!ModelState.IsValid)
            {
                loginInfoDto = new LoginInfoDto();
                loginInfoDto.errorMessege = new List<string>();

                return View("Za200", loginInfoDto);
            }
            else
            {
                // パスワードをSHA256暗号化する
                loginInfoDto.txtPwd = Encrypt(loginInfoDto.txtPwd);

                // txtMail、txtPwdをSELECT
                loginInfoDto = SelectLogin(loginInfoDto.txtMail, loginInfoDto.txtPwd);

                // txtMailまたはtxtPwd誤りがある場合
                if (loginInfoDto == null)
                {
                    loginInfoDto = new LoginInfoDto();
                    loginInfoDto.errorMessege = new List<string>();

                    // エラ〜メッセージ設定
                    loginInfoDto.errorMessege.Add("メールアドレスまたはパスワードの入力に誤りがあるか登録されていません。");

                    return View("Za200", loginInfoDto);
                }

                // Loginデータをセッションに格納
                HttpContext.Session.SetString("memberId", loginInfoDto.txtId);
                HttpContext.Session.SetString("mailAddr", loginInfoDto.txtMail);
                HttpContext.Session.SetString("authorityCd", loginInfoDto.txtAuthorityCd);
                HttpContext.Session.SetString("companyCd", loginInfoDto.txtCompanyCd);

                return RedirectToAction("event_initial", "Za100");
            }
        }

        /// <summary>
        /// パスワードをSHA256暗号化する
        /// 内容：passwordをSHA256変換する
        /// </summary>
        /// <returns>パスワード</returns>
        public String Encrypt(string txtPwd)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(txtPwd);
            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] encryptBytes = sha256Managed.ComputeHash(passwordBytes);

            string encryptString = Convert.ToBase64String(encryptBytes);

            return encryptString;
        }

        /// <summary>
        /// Email、PasswordをSELECTする
        /// 内容：入力項目（Email、Password）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public LoginInfoDto SelectLogin(string txtMail, string txtPwd)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            loginInfoDto = context.SelectLoginZa200(txtMail, txtPwd);
            return loginInfoDto;

        }
    }
}