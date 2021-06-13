using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

namespace Hanyokessai.Controllers
{
    public class Zb300Controller : Controller
    {
        /// <summary>
        /// default action 
        /// 内容：初期表示画面（Index　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult event_Initial(string kaiinMail)
        {
            // 初期化
            Zb300Dto zb300Dto = new Zb300Dto();

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            //　会員情報取得
            zb300Dto = context.GetUserInfoByMailZb300(kaiinMail);
            if (zb300Dto.txtimgStamp!=null)
            {
                zb300Dto.stampFlg = true;
                zb300Dto.txtimgStamp = "data: image / png; base64," + zb300Dto.txtimgStamp;
            }
            else
            {
                zb300Dto.stampFlg = false;
            }
            zb300Dto.txtEnterDate = zb300Dto.txtEnterDate.Substring(0, 10);

            // 会社名,所属,職位を取得
            zb300Dto = zb300Restart(zb300Dto);
            zb300Dto.errorMessege = new List<string>();

            return View("Zb300", zb300Dto);
        }

        /// <summary>
        /// 所属,職位を取得する 
        /// 内容：会社名,所属,職位を取得する。
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        public Zb300Dto zb300Restart(Zb300Dto zb300Dto)
        {

            Zb300Dto zb300Dto1 = new Zb300Dto();

            // 所属1を取得
            zb300Dto1 = SelectCompanyDepartment1List();
            zb300Dto.txtDep1CdList = zb300Dto1.txtDep1CdList;
            zb300Dto.txtDep1NameList = zb300Dto1.txtDep1NameList;

            // 所属2情報を取得
            zb300Dto1 = SelectCompanyDepartment2List();
            zb300Dto.txtDep2CdList = zb300Dto1.txtDep2CdList;
            zb300Dto.txtDep2CdNameList = zb300Dto1.txtDep2CdNameList;

            // 職位情報を取得
            zb300Dto1 = SelectCompanyPositionList();
            zb300Dto.txtComPosCdList = zb300Dto1.txtComPosCdList;
            zb300Dto.txtComPosCdNameList = zb300Dto1.txtComPosCdNameList;

            return zb300Dto;
        }

        /// <summary>
        /// 所属1をSELECTする
        /// 内容：所属1（dep1_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Zb300Dto SelectCompanyDepartment1List()
        {

            Zb300Dto zb300Dto = new Zb300Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment1ListZa300();

            zb300Dto.txtDep1CdList = za300Dto.txtDep1CdList;
            zb300Dto.txtDep1NameList = za300Dto.txtDep1NameList;

            return zb300Dto;

        }

        /// <summary>
        /// 所属2をSELECTする
        /// 内容：所属2（dep2_cd, dep1_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Zb300Dto SelectCompanyDepartment2List()
        {

            Zb300Dto zb300Dto = new Zb300Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectDepartment2ListZa300();

            zb300Dto.txtDep2CdList = za300Dto.txtDep2CdList;
            zb300Dto.txtDep2CdNameList = za300Dto.txtDep2CdNameList;

            return zb300Dto;

        }

        /// <summary>
        /// 職位をSELECTする
        /// 内容：職位（pos_cd, pos_name）をSELECTする
        /// </summary>
        /// <returns>ログイン情報Dto</returns>
        [HttpPost]
        public Zb300Dto SelectCompanyPositionList()
        {

            Zb300Dto zb300Dto = new Zb300Dto();
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            Za300Dto za300Dto = context.SelectCompanyPositionListZa300();

            zb300Dto.txtComPosCdList = za300Dto.txtComPosCdList;
            zb300Dto.txtComPosCdNameList = za300Dto.txtComPosCdNameList;

            return zb300Dto; ;

        }

        [HttpPost]
        public IActionResult event_Modify(Zb300Dto zb300Dto)
        {
            // チェック処理がfalseの場合
            if (!ModelState.IsValid)
            {
                zb300Dto.errorMessege = new List<string>();

                // 会社名,所属,職位を取得
                zb300Dto = zb300Restart(zb300Dto);

                return View("Zb300", zb300Dto);
            }
            else
            {
                // 会員加入Dtoに固定値を格納

                zb300Dto.txtUpdateid = HttpContext.Session.GetString("memberId");
                zb300Dto.txtUpdateDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                //　DB Connect
                DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
                context.UpdateUserInfoByMailZb300(zb300Dto);

                return RedirectToAction("event_initial", "Zb100");
            }
        }

        public IActionResult event_Reset_Password(string kaiinMail)
        {
            //　DB Connect
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;

            string password = new string("password");
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] encryptBytes = sha256Managed.ComputeHash(passwordBytes);

            password = Convert.ToBase64String(encryptBytes);

            string upDateID = HttpContext.Session.GetString("memberId");

            context.ResetPassword(upDateID, kaiinMail, password);


            return RedirectToAction("event_Initial", "Zb100");
        }

        public IActionResult event_Cancel()
        {
            return RedirectToAction("event_Initial", "Zb100");
        }
    }
}

