using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;

namespace Hanyokessai.Controllers
{
    public class TestController : Controller
    {
        /// <summary>
        /// default action 
        /// 内容：初期表示画面（Index　ページ）に遷移する。
        /// </summary>
        /// <returns>default page</returns>
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("KEY", 1);
            return View();
        }

        /// <summary>
        /// 検索ボータンを押下
        /// 内容：input値を比較した上、Test2 page　or 　Test3 pageに遷移する。
        /// </summary>
        /// <returns>Test2 page　or　Test3 page</returns>
        public IActionResult Test2(string parm)
        {
            if (parm == "1")
            {
                return View("Test3");

            }

            return View();
        }

        /// <summary>
        /// 会員加入ボータンを押下
        /// 内容：内容入力ページに遷移する。
        /// </summary>
        /// <returns>Test3 page</returns>
        public IActionResult Test3()
        {
            return View();
        }

        /// <summary>
        /// 会員一覧ボータンを押下
        /// 内容：会員一覧ページに遷移する。
        /// </summary>
        /// <returns>Test4 page</returns>
        public IActionResult Test4()
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            List<Data> list = context.GetData();
            return View(list);
        }

        /// <summary>
        /// (Test3 page)内容入力の会員登録を押下
        /// 内容：DBに登録してIndexページに遷移する。
        /// </summary>
        /// <returns>Index page</returns>
        [HttpPost]
        public IActionResult Test5(Data user)
        {
            DbService context = HttpContext.RequestServices.GetService(typeof(DbService)) as DbService;
            context.InsertData(user);
            return View("Index");

        }

        public IActionResult Test6(Data user)
        {
            //the path of the file
            string filePath = "C:\\Hanyokessai\\FOS交通代(20年8月)jiseokho.xlsx";

            //create a fileinfo object of an excel file on the disk
            FileInfo file = new FileInfo(filePath);

            //create a new Excel package from the file
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                //create an instance of the the first sheet in the loaded file
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                //get the image from disk
                using (System.Drawing.Image image = System.Drawing.Image.FromFile("C:\\Hanyokessai\\\\123.png"))
                {
                    var excelImage = worksheet.Drawings.AddPicture("My Logo", image);

                    //add the image to row 20, column E
                    excelImage.SetPosition(2, 0, 9, 40);
                    excelImage.SetSize(30, 50);
                }

                //save the changes
                excelPackage.Save();

            }
            return View("Index");
        }
    }
}

