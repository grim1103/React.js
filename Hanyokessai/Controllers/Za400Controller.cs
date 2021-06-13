using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
//using System;
//using OfficeOpenXml;
//using MySql.Data.MySqlClient;
//using System.Drawing;

namespace Hanyokessai.Controllers
{
    public class Za400Controller : Controller
    {
        /// <summary>
        /// default action 
        /// 内容：初期表示画面（Index　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Initial()
        {
            // 初期化
            Za400Dto za400Dto = new Za400Dto();

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            // セッションemail取得
            string email = HttpContext.Session.GetString("mailAddr");
            //　会員情報取得
            za400Dto = context.GetUserInfoByMailZa400(email);
            za400Dto.txtimgStamp = "data: image / png; base64," + za400Dto.txtimgStamp;

            // 会社名,所属,職位を取得
            za400Dto = za400Restart(za400Dto);
            za400Dto.errorMessege = new List<string>();

            return View("Za400", za400Dto);
        }

        /// <summary>
        /// 所属,職位を取得する 
        /// 内容：会社名,所属,職位を取得する。
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        public Za400Dto za400Restart(Za400Dto za400Dto)
        {

            Za400Dto za400Dto1 = new Za400Dto();

            // 所属1を取得
            za400Dto1 = SelectCompanyDepartment1List();
            za400Dto.txtDep1CdList = za400Dto1.txtDep1CdList;
            za400Dto.txtDep1NameList = za400Dto1.txtDep1NameList;

            // 所属2情報を取得
            za400Dto1 = SelectCompanyDepartment2List();
            za400Dto.txtDep2CdList = za400Dto1.txtDep2CdList;
            za400Dto.txtDep2CdNameList = za400Dto1.txtDep2CdNameList;

            // 職位情報を取得
            za400Dto1 = SelectCompanyPositionList();
            za400Dto.txtComPosCdList = za400Dto1.txtComPosCdList;
            za400Dto.txtComPosCdNameList = za400Dto1.txtComPosCdNameList;

            return za400Dto;
        }

        /// <summary>
        /// 所属1をSELECTする
        /// 内容：所属1（dep1_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za400Dto SelectCompanyDepartment1List()
        {

            Za400Dto za400Dto = new Za400Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment1ListZa300();

            za400Dto.txtDep1CdList = za300Dto.txtDep1CdList;
            za400Dto.txtDep1NameList = za300Dto.txtDep1NameList;

            return za400Dto;

        }

        /// <summary>
        /// 所属2をSELECTする
        /// 内容：所属2（dep2_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za400Dto SelectCompanyDepartment2List()
        {

            Za400Dto za400Dto = new Za400Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment2ListZa300();

            za400Dto.txtDep2CdList = za300Dto.txtDep2CdList;
            za400Dto.txtDep2CdNameList = za300Dto.txtDep2CdNameList;

            return za400Dto;

        }

        /// <summary>
        /// 職位をSELECTする
        /// 内容：職位（pos_cd, pos_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za400Dto SelectCompanyPositionList()
        {

            Za400Dto za400Dto = new Za400Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectCompanyPositionListZa300();

            za400Dto.txtComPosCdList = za300Dto.txtComPosCdList;
            za400Dto.txtComPosCdNameList = za300Dto.txtComPosCdNameList;

            return za400Dto; ;

        }

        [HttpPost]
        public IActionResult event_Modify(Za400Dto za400Dto, IFormFile imgStamp)
        {
            string text = null;

            // チェック処理がfalseの場合
            if (!ModelState.IsValid)
            {

                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

                // セッションemail取得
                string email = HttpContext.Session.GetString("mailAddr");
                //　会員情報取得
                za400Dto = context.GetUserInfoByMailZa400(email);
                za400Dto.txtimgStamp = "data: image / png; base64," + za400Dto.txtimgStamp;

                // 会社名,所属,職位を取得
                za400Dto = za400Restart(za400Dto);

                za400Dto.errorMessege = new List<string>();

                // 会社名,所属,職位を取得
                za400Dto = za400Restart(za400Dto);

                return View("Za400", za400Dto);
            }
            else
            {

                // 論理項目チェック
                za400Dto = ErrorCheck(za400Dto, imgStamp);

                // エラーメッセージが存在する場合
                if (0 != za400Dto.errorMessege.Count)
                {
                    return View("Za300", za400Dto);
                }

                // 会員加入Dtoに固定値を格納
                za400Dto.txtUpdateid = za400Dto.member_id;
                za400Dto.txtPwd = Encrypt(za400Dto.txtPwd);
                za400Dto.txtUpdateDate = DateTime.Now.ToString("yyyyMMddHHmmss");


                // 押印ファイルを設定しない場合
                if (za400Dto.imgStamp == null)
                {
                    za400Dto.txtStampCode = "0";
                    text = "";
                }
                else
                {
                    za400Dto.txtStampCode = "1";

                    // fileSize格納
                    za400Dto.fileSize = (UInt32)imgStamp.Length;

                    // txtStampCode設定
                    String fileImage = Common.filUpload(imgStamp);

                    // fileRead
                    Stream a = imgStamp.OpenReadStream();

                    // Stream → byte
                    byte[] b = GetByteArrayFromStream(a);

                    // byte → base64 
                    text = Convert.ToBase64String(b);
                }

                //　DB Connect
                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                context.UpdateUserInfoByMailZa400(za400Dto, text);

                return RedirectToAction("event_initial", "Za600");
            }
        }

        /// <summary>
        /// 論理チェックを行う
        /// 内容：論理チェックを行う
        /// </summary>
        /// <returns>errorMessege</returns>
        public Za400Dto ErrorCheck(Za400Dto za400Dto, IFormFile imgStamp)
        {
            za400Dto.errorMessege = new List<string>();

            // 押印ファイルがない場合
            if (imgStamp != null)
            {
                // imgStamp設定
                za400Dto.imgStamp = imgStamp;

                // 拡張子チェック
                String extension = Path.GetExtension(imgStamp.FileName);
                // エラーメッセージを設定
                if (!(".jpg".Equals(extension) || ".jpeg".Equals(extension) || ".png".Equals(extension)))
                {
                    za400Dto.errorMessege.Add("押印ファイルに誤りがあります。");
                }
            }

            return za400Dto;
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
        /// 会員情報照会画面にて遷移する 
        /// 内容：会員情報照会画面にて遷移する。
        /// </summary>
        /// <returns>　会員情報照会画面　</returns>
        public IActionResult event_Cancel()
        {

            return RedirectToAction("event_Initial", "Za600");
        }
        /// <summary>
        /// 会員脱会画面にて遷移する
        /// 内容：会員脱会画面にて遷移する
        /// </summary>
        /// <returns></returns>
        public IActionResult event_UnRegister()
        {
            HttpContext.Session.SetString("gamenID", "Za400");
            return RedirectToAction("event_Initial", "Za500");
        }
    }
}

