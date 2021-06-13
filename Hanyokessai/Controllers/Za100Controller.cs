using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Hanyokessai.Models;

namespace Hanyokessai.Controllers
{
    public class Za100Controller : Controller
    {
        public IActionResult event_initial()
        {
            return View("Za100");
        }

        /// <summary>
        /// 会員照会画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_usersyokai()
        {
            // アクション名：event_Initial
            // コントローラー名：会員情報紹介
            return RedirectToAction("event_Initial", "Za600");
        }

        /// <summary>
        /// テンプレート一覧画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_template()
        {
            // アクション名：event_Initial
            // コントローラー名：テンプレート管理
            return RedirectToAction("event_Initial", "Ta200");
        }

        /// <summary>
        /// 帳票一覧画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_tyohyo()
        {
            // アクション名：event_Initial
            // コントローラー名：帳票一覧
            return RedirectToAction("event_Initial", "Ca300");
        }

        /// <summary>
        /// 決済管理画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_kessai()
        {
            // アクション名：event_Initial
            // コントローラー名：決済管理
            return RedirectToAction("event_Initial", "Ka200");
        }

        /// <summary>
        /// ユーザ情報管理画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_userkanri()
        {
            // アクション名：event_Initial
            // コントローラー名：ユーザー情報管理一覧
            return RedirectToAction("event_Initial", "Zb100");
        }

        /// <summary>
        /// 押印グループ管理画面に遷移する。
        /// </summary>
        /// <returns></returns>
        public IActionResult event_ouin()
        {
            // アクション名：event_Initial
            // コントローラー名：押印グループ管理
            return RedirectToAction("event_Initial", "Zb400");
        }

    }
}
