using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Hanyokessai.Controllers
{
    public class Za300Controller : Controller
    {

        Za300Dto za300Dto;                          // 会社名リスト
        String serialNumber;                        // 連番
        String memberId;                            // メンバーId

        /// <summary>
        /// default action 
        /// 内容：会員加入画面（会員加入　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Initial()
        {
            za300Dto = new Za300Dto();
            za300Dto.errorMessege = new List<string>();

            // 会社名,所属,職位を取得
            za300Dto = za300Restart(za300Dto);

            return View("Za300", za300Dto);
        }

        /// <summary>
        /// default action 
        /// 内容：main画面（main　ページ）に遷移する。
        /// </summary>
        /// <returns>main page</returns>
        public IActionResult event_Register(Za300Dto za300Dto, IFormFile imgStamp)
        {
            string text = null;

            // チェック処理がfalseの場合
            if (!ModelState.IsValid)
            {
                za300Dto.errorMessege = new List<string>();

                // imgStamp設定
                za300Dto.imgStamp = imgStamp;

                // 会社名,所属,職位を取得
                za300Dto = za300Restart(za300Dto);

                return View("Za300", za300Dto);
            }
            else
            {

                // 論理項目チェック
                za300Dto = ErrorCheck(za300Dto, imgStamp);

                // エラーメッセージが存在する場合
                if (0 != za300Dto.errorMessege.Count)
                {
                    return View("Za300", za300Dto);
                }

                // 会員加入Dtoに固定値を格納
                za300Dto.txtId = GetTxtId(za300Dto.txtCompanyCode);
                za300Dto.txtRegisterId = za300Dto.txtId;
                za300Dto.txtPwd = Encrypt(za300Dto.txtPwd);
                za300Dto.txtAuthorityCode = "0";
                za300Dto.txtDelCode = "0";
                za300Dto.txtRegisterDay = DateTime.Now.ToString("yyyyMMddHHmmss");

                // セッションにてに会員加入Dtoを格納
                HttpContext.Session.SetString("memberId", za300Dto.txtId);
                HttpContext.Session.SetString("mailAddr", za300Dto.txtMail);
                HttpContext.Session.SetString("authorityCd", za300Dto.txtAuthorityCode);
                HttpContext.Session.SetString("companyCd", za300Dto.txtCompanyCode);

                // 押印ファイルを設定しない場合
                if (za300Dto.imgStamp == null)
                {
                    za300Dto.txtStampCode = "0";
                    text = "";
                }
                else
                {
                    za300Dto.txtStampCode = "1";

                    // fileSize格納
                    za300Dto.fileSize = (UInt32)imgStamp.Length;

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
                context.InsertMemberRegister(za300Dto, text);

                return RedirectToAction("event_initial", "Za100");

            }
        }

        /// <summary>
        /// 会社名,所属,職位を取得する 
        /// 内容：会社名,所属,職位を取得する。
        /// </summary>
        /// <returns>￥ログイン情報Dto</returns>
        public Za300Dto za300Restart(Za300Dto za300Dto)
        {

            Za300Dto za300Dto1 = new Za300Dto();

            // 会社コードを取得
            za300Dto.txtComCdList = SelectCompanyName();

            // 所属1を取得
            za300Dto1 = SelectCompanyDepartment1List();
            za300Dto.txtDep1CdList = za300Dto1.txtDep1CdList;
            za300Dto.txtDep1NameList = za300Dto1.txtDep1NameList;

            // 所属2情報を取得
            za300Dto1 = SelectCompanyDepartment2List();
            za300Dto.txtDep2CdList = za300Dto1.txtDep2CdList;
            za300Dto.txtDep2CdNameList = za300Dto1.txtDep2CdNameList;

            // 職位情報を取得
            za300Dto1 = SelectCompanyPositionList();
            za300Dto.txtComPosCdList = za300Dto1.txtComPosCdList;
            za300Dto.txtComPosCdNameList = za300Dto1.txtComPosCdNameList;

            return za300Dto;
        }

        /// <summary>
        /// ログイン画面にて遷移する 
        /// 内容：ログイン画面にて遷移する。
        /// </summary>
        /// <returns>　ログイン画面　</returns>
        public IActionResult event_Cancel()
        {

            return RedirectToAction("event_Initial", "Za200");
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
        /// 会員IDを取得する 
        /// 内容：会員ID(会社コード＋連番（７桁）)を取得する。
        /// </summary>
        /// <returns>memberId</returns>
        public String GetTxtId(String txtCompanyCode)
        {

            //　連番を取得
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            serialNumber = context.SelectSerialNumber();
            memberId = txtCompanyCode + serialNumber;

            return memberId;
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
        /// 会社名をSELECTする
        /// 内容：会社名（com_cd）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public List<string> SelectCompanyName()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<string> comCdList = context.SelectcompanyNameZa300();

            return comCdList;

        }

        /// <summary>
        /// 所属1をSELECTする
        /// 内容：所属1（dep1_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za300Dto SelectCompanyDepartment1List()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment1ListZa300();

            return za300Dto;

        }

        /// <summary>
        /// 所属2をSELECTする
        /// 内容：所属2（dep2_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za300Dto SelectCompanyDepartment2List()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment2ListZa300();

            return za300Dto;

        }

        /// <summary>
        /// 職位をSELECTする
        /// 内容：職位（pos_cd, pos_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za300Dto SelectCompanyPositionList()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectCompanyPositionListZa300();

            return za300Dto;

        }

        /// <summary>
        /// 職位をSELECTする
        /// 内容：職位（pos_cd, pos_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Za300Dto InsertMemberRegister()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectCompanyPositionListZa300();

            return za300Dto;

        }

        /// <summary>
        /// メールをSELECTする
        /// 内容：メールをSELECTし、mailを返す
        /// </summary>
        /// <returns>errorMessege</returns>
        public List<String> SelectVerifyEmail(string email)
        {
            List<String> errorMessege = new List<string>();

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            email = context.SelectEmailZa300(email);

            // エラーメッセージを設定
            if (!"".Equals(email))
            {
                errorMessege.Add("メールアドレスが既に存在います。再確認お願いします。");
            }

            return errorMessege;
        }

        /// <summary>
        /// メールをSELECTする
        /// 内容：メールをSELECTし、mailを返す
        /// </summary>
        /// <returns>errorMessege</returns>
        public Za300Dto ErrorCheck(Za300Dto za300Dto, IFormFile imgStamp)
        {
            String mail = za300Dto.txtMail;
            za300Dto = za300Restart(za300Dto);

            // メール重複チェック
            za300Dto.errorMessege = SelectVerifyEmail(mail);

            // 押印ファイルがない場合
            if (imgStamp != null)
            {
                // imgStamp設定
                za300Dto.imgStamp = imgStamp;

                // 拡張子チェック
                String extension = Path.GetExtension(imgStamp.FileName);
                // エラーメッセージを設定
                if (!(".jpg".Equals(extension) || ".jpeg".Equals(extension) || ".png".Equals(extension)))
                {
                    za300Dto.errorMessege.Add("押印情報有無に誤りがあります。");
                }
            }

            return za300Dto;
        }
    }
}