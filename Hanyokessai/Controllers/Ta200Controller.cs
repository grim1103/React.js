using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hanyokessai.Controllers
{
    public class Ta200Controller : Controller
    {
        /// テンプレート一覧画面に転移する。
        public IActionResult event_Initial()
        {
            string companyCd = HttpContext.Session.GetString("companyCd");

            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<Ta200Dto> ta200 = context.SelectTempletTa200(companyCd);

            foreach (Ta200Dto dto in ta200)
            {
                if (!Common.IsNullOrEmpty(dto.txtUpdName))
                {
                    Ta200MemDto memDto = context.SelectMemberTa200(dto.txtUpdName);
                    dto.txtUpdName = memDto.txtUpdName;
                }
            }

            return View(Constants.View.TA200, ta200);
        }

        public IActionResult event_Cancel()
        {

            return RedirectToAction("event_initial", Constants.View.ZA100);
        }

        public IActionResult event_TmpInquiry(string TempleteId)
        {
            TempData["TempleteId"] = TempleteId;
            return RedirectToAction("event_initial", Constants.View.TA300);
        }

        public IActionResult event_TmpDownload(string fileName, string binaryFile, string fileSize)
        {
            UInt32 templeteFileSize = Convert.ToUInt32(fileSize);
            Common.templateDownload(fileName, binaryFile, templeteFileSize);
            return RedirectToAction("event_Initial", Constants.View.TA200);
        }

        public IActionResult event_up(string TempleteId)
        {
            return RedirectToAction("event_initial", Constants.View.TA100);
        }
    }
}

