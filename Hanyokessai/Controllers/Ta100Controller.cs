using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanyokessai.Controllers
{

    public class Ta100Controller : Controller
    {

        /// <summary>
        /// テンプレート登録ページに遷移する
        /// 内容：テンプレート登録ページに遷移する
        /// </summary>
        /// <returns>View</returns>
        public IActionResult event_Initial()
        {

            // テンプレート登録ページにて必要な情報取得
            Ta100Dto ta100Dto = new Ta100Dto();
            ta100Dto = ta100Start(ta100Dto);
            ta100Dto.errorMessege = new List<string>();

            return View(Constants.View.TA100, ta100Dto);
        }

        /// <summary>
        /// 会社名をSELECTする
        /// 内容：会社名（com_cd）をSELECTする
        /// </summary>
        /// <returns>selComNameList</returns>
        [HttpPost]
        public List<string> SelectCompanyName()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<string> selComNameList = context.SelectcompanyNameZa300();

            return selComNameList;

        }

        /// <summary>
        /// 職位をSELECTする
        /// 内容：職位をSELECTする
        /// </summary>
        /// <returns>ta100Dto</returns>
        [HttpPost]
        public Ta100Dto SelectPositionName()
        {

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Ta100Dto ta100Dto = context.SelectPositionName();

            return ta100Dto;

        }

        /// <summary>
        /// キャンセルボタンを押下
        /// 内容：Ta200のページに戻る
        /// </summary>
        /// <returns>RedirectToAction</returns>
        public IActionResult temcancle()
        {
            Ta100Dto ta100Dto = new Ta100Dto();
            // テンプレート登録ページにて必要な情報取得
            ta100Dto = ta100Start(ta100Dto);

            return RedirectToAction("event_Initial", Constants.View.TA200);
        }

        /// <summary>
        /// Ta100のページを開く前、パラメータを設定する
        /// 内容：ta100Dtoにて必要な項目を設定する
        /// </summary>
        /// <returns>ta100Dto</returns>
        public Ta100Dto ta100Start(Ta100Dto ta100Dto)
        {

            // 職位取得
            ta100Dto = SelectPositionName();
            //セッション取得
            ta100Dto.txtTemAuthor = HttpContext.Session.GetString("mailAddr");
            // 会社コードを取得
            ta100Dto.selComNameList = SelectCompanyName();
            // 日付設定
            ta100Dto.dateAuthorDate = DateTime.Now.ToString("yyyy-MM-dd");

            return ta100Dto;
        }

        /// <summary>
        /// 登録ボタンを押下
        /// 内容：入力内容をINSERTする
        /// </summary>
        /// <returns>ViewまたはRedirectToAction</returns>
        [HttpPost]
        public IActionResult temregister(Ta100Dto ta100Dto, IFormFile tmpfile)
        {

            string tmpfileText = null;
            // チェック処理がfalseの場合
            if (!ModelState.IsValid)
            {
                ta100Dto = ta100Start(ta100Dto);
                ta100Dto.errorMessege = new List<string>();

                return View(Constants.View.TA100, ta100Dto);

            }
            else
            {

                // 論理項目チェック
                ta100Dto = ErrorCheck(ta100Dto, tmpfile);

                // エラーメッセージが存在する場合
                if (0 != ta100Dto.errorMessege.Count)
                {
                    List<string> errorMessege = ta100Dto.errorMessege;
                    string checkAbleFlg1 = ta100Dto.checkAbleFlg1;
                    string checkAbleFlg2 = ta100Dto.checkAbleFlg2;
                    string checkAbleFlg3 = ta100Dto.checkAbleFlg3;
                    string checkAbleFlg4 = ta100Dto.checkAbleFlg4;
                    string checkAbleFlg5 = ta100Dto.checkAbleFlg5;
                    string checkAbleFlg6 = ta100Dto.checkAbleFlg6;
                    string checkAbleFlg7 = ta100Dto.checkAbleFlg7;
                    string checkAbleFlg8 = ta100Dto.checkAbleFlg8;
                    string checkAbleFlg9 = ta100Dto.checkAbleFlg9;
                    string checkAbleFlg10 = ta100Dto.checkAbleFlg10;

                    ta100Dto = ta100Start(ta100Dto);
                    ta100Dto.checkAbleFlg1 = checkAbleFlg1;
                    ta100Dto.checkAbleFlg2 = checkAbleFlg2;
                    ta100Dto.checkAbleFlg3 = checkAbleFlg3;
                    ta100Dto.checkAbleFlg4 = checkAbleFlg4;
                    ta100Dto.checkAbleFlg5 = checkAbleFlg5;
                    ta100Dto.checkAbleFlg6 = checkAbleFlg6;
                    ta100Dto.checkAbleFlg7 = checkAbleFlg7;
                    ta100Dto.checkAbleFlg8 = checkAbleFlg8;
                    ta100Dto.checkAbleFlg9 = checkAbleFlg9;
                    ta100Dto.checkAbleFlg10 = checkAbleFlg10;
                    ta100Dto.errorMessege = errorMessege;
                    return View(Constants.View.TA100, ta100Dto);
                }

                // ta100Dtoに格納
                ta100Dto.selTemType = GetTmpId(ta100Dto.selComName);
                ta100Dto.tmpFileSize = (UInt32)tmpfile.Length;
                ta100Dto.txtFileName = tmpfile.FileName;
                ta100Dto.txtRegId = HttpContext.Session.GetString("memberId");
                ta100Dto.txtDelCd = "0";
                ta100Dto.txtRegDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                // txtStampCode設定
                String fileImage = Common.filUpload(tmpfile);

                // fileRead
                Stream a = tmpfile.OpenReadStream();

                // Stream → byte
                byte[] b = GetByteArrayFromStream(a);

                // byte → base64 
                tmpfileText = Convert.ToBase64String(b);

                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                context.InsertTemplateRegister(ta100Dto, tmpfileText);

                return RedirectToAction("event_Initial", Constants.View.TA200);
            }
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
        /// テンプレートIDを取得する 
        /// 内容：テンプレートID(会社コード＋連番（3桁）)を取得する。
        /// </summary>
        /// <returns>tepId</returns>
        public String GetTmpId(String txtCompanyCode)
        {

            //　連番を取得
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            String serialNumber = context.SelectTmpSerialNumber();
            String tepId = txtCompanyCode + serialNumber;

            return tepId;
        }

        /// <summary>
        /// 相関項目チェック
        /// 内容：相関項目チェックを行う。
        /// </summary>
        /// <returns>errorMessege</returns>
        public Ta100Dto ErrorCheck(Ta100Dto ta100Dto, IFormFile formFile)
        {

            List<String> valCheckAble = new List<String>();
            List<String> valCheckSelApprover = new List<String>();
            List<String> valCheckApprovalMail = new List<String>();
            List<String> valChecktxtApproPlace = new List<String>();
            ta100Dto.errorMessege = new List<string>();

            Regex emailCheck = new System.Text.RegularExpressions.Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Regex placeCheck = new System.Text.RegularExpressions.Regex("^[A-Za-z0-9+]*$");

            //　チェック項目を格納する
            valCheckAble.Add(ta100Dto.checkAbleFlg1);
            valCheckAble.Add(ta100Dto.checkAbleFlg2);
            valCheckAble.Add(ta100Dto.checkAbleFlg3);
            valCheckAble.Add(ta100Dto.checkAbleFlg4);
            valCheckAble.Add(ta100Dto.checkAbleFlg5);
            valCheckAble.Add(ta100Dto.checkAbleFlg6);
            valCheckAble.Add(ta100Dto.checkAbleFlg7);
            valCheckAble.Add(ta100Dto.checkAbleFlg8);
            valCheckAble.Add(ta100Dto.checkAbleFlg9);
            valCheckAble.Add(ta100Dto.checkAbleFlg10);

            valCheckSelApprover.Add(ta100Dto.selApprover1);
            valCheckSelApprover.Add(ta100Dto.selApprover2);
            valCheckSelApprover.Add(ta100Dto.selApprover3);
            valCheckSelApprover.Add(ta100Dto.selApprover4);
            valCheckSelApprover.Add(ta100Dto.selApprover5);
            valCheckSelApprover.Add(ta100Dto.selApprover6);
            valCheckSelApprover.Add(ta100Dto.selApprover7);
            valCheckSelApprover.Add(ta100Dto.selApprover8);
            valCheckSelApprover.Add(ta100Dto.selApprover9);
            valCheckSelApprover.Add(ta100Dto.selApprover10);

            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail1);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail2);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail3);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail4);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail5);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail6);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail7);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail8);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail9);
            valCheckApprovalMail.Add(ta100Dto.txtApprovalMail10);

            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace1);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace2);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace3);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace4);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace5);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace6);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace7);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace8);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace9);
            valChecktxtApproPlace.Add(ta100Dto.txtApproPlace10);

            // 相関チェックを行う
            for (int i = 0; i < valCheckAble.Count; i++)
            {

                if ("0".Equals(valCheckAble[i]))
                {
                    int mailTextCount = valCheckApprovalMail[i].Split(' ').Length;
                    int placeTextCount = valChecktxtApproPlace[i].Split(' ').Length;
                    Boolean emailMatch = emailCheck.IsMatch(valCheckApprovalMail[i]);
                    Boolean placeMatch = placeCheck.IsMatch(valChecktxtApproPlace[i]);

                    if ("".Equals(valCheckSelApprover[i]))
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "押印者を入力してください。");
                    }
                    // メールの未入力チェック
                    if ("".Equals(valCheckApprovalMail[i]))
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "Emailを入力してください。");
                    }
                    // メールの桁数チェック
                    if (mailTextCount > 50)
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "Emailは50桁以下に入力してください。");
                    }
                    // メールの形式チェック
                    if (!emailMatch)
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "Email形式で入力してください。");
                    }
                    //　押印位置の未入力チェック
                    if (valChecktxtApproPlace[i] == null)
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "押印位置を入力してください。");
                    }
                    // 押印位置の桁数チェック
                    if (placeTextCount > 10)
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "Emailは50桁以下に入力してください。");
                    }
                    // 押印位置の形式チェック
                    if (!placeMatch)
                    {
                        ta100Dto.errorMessege.Add("決裁者" + (i + 1) + "押印位置は記号使えません。");
                    }
                }
            }

            return ta100Dto;
        }
    }
}