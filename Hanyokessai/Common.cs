using Hanyokessai.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Hanyokessai
{
    public static class Common
    {
        private const string FILE_PATH = "C:\\HanyokessaiClient\\";
       
        /// <summary>
        /// File Upload
        /// </summary>
        /// <param name="imgStamp"></param>
        /// <returns></returns>
        internal static string filUpload(IFormFile imgStamp)
        {
            if (imgStamp == null) {
                return "";
            }
            Stream imgFile = new MemoryStream();
            // ファイルをストリームに変換する
            imgFile = imgStamp.OpenReadStream();
            using (MemoryStream ms = new MemoryStream())
            {
                // メモリーストリームに変換する。
                imgFile.CopyTo(ms);
                // バイト配列に変換する。
                byte[] imgString = ms.ToArray();
                // 文字列に変換して返却する。
                return Convert.ToBase64String(imgString);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// テンプレートをダウンロードする。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBinary"></param>
        /// <param name="templeteFileSize"></param>
        internal static void templateDownload(string fileName, string fileBinary, UInt32 templeteFileSize)
        {
            // 文字列をバイト配列に変換する。
            byte[] fileBinaryArray = Convert.FromBase64String(fileBinary);
            // パスチェック
            filePathCheck();
            string fileAll = FILE_PATH + fileName;
            // ファイルを生成
            FileStream fs = new FileStream(fileAll, FileMode.OpenOrCreate, FileAccess.Write);
            // 中身を書く
            fs.Write(fileBinaryArray, 0, (int)templeteFileSize);
            fs.Close();
        }

        /// <summary>
        /// レポートをダウンロードする。
        /// </summary>
        /// <param name="templeteID"></param>
        /// <param name="downloadDto"></param>
        /// <param name="fileName"></param>
        /// <param name="fileBinary"></param>
        /// <param name="templeteFileSize"></param>
        internal static void reportDownload(string templeteID, DownloadDto downloadDto, string fileName, string fileBinary, UInt32 templeteFileSize)
        {
            // 文字列をバイト配列に変換する。
            byte[] fileBinaryArray = Convert.FromBase64String(fileBinary);
            // パスチェック
            filePathCheck();
            string fileAll = FILE_PATH + fileName;
            // ファイルを生成
            FileStream fs = new FileStream(fileAll, FileMode.OpenOrCreate, FileAccess.Write);
            // 中身を書く
            fs.Write(fileBinaryArray, 0, (int)templeteFileSize);
            fs.Close();
            // 押印する。
            fixStamp(downloadDto, fileAll);
        }

        /// <summary>
        /// 印鑑を押印する。
        /// </summary>
        /// <param name="downloadDto"></param>
        /// <param name="fileAll"></param>
        internal static void fixStamp(DownloadDto downloadDto, string fileAll)
        {
            // 繰り返す条件
            int end = downloadDto.stampInfo.Length;
            // ファイルを読み込む
            FileInfo file = new FileInfo(fileAll);
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                // 一番目のシート
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                for (int i = 0; i < end; i++)
                {
                    // 押印情報がある場合
                    if (!Common.IsNull(downloadDto.stampInfo[i]))
                    {
                        // 情報設定
                        string stampInfo = downloadDto.stampInfo[i];
                        // サイズ
                        string stampInfoSize = downloadDto.stampInfoSize[i];
                        // 位置
                        string stampInfoPlace = downloadDto.stampInfoPlace[i];
                        UInt32 templeteFileSize = Convert.ToUInt32(stampInfoSize);
                        // イメージをダウンロード
                        Common.templateDownload(stampInfoPlace + ".png", stampInfo, templeteFileSize);
                        // イメージを読み込む
                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(FILE_PATH + stampInfoPlace + ".png"))
                        {
                            // イメージのオブジェクトを生成
                            var excelImage = worksheet.Drawings.AddPicture(stampInfoPlace + sysDate(), image);
                            // 張り付ける位置を取る
                            int[] rowcol = stampPositionChange(stampInfoPlace);
                            // イメージを張り付ける
                            excelImage.SetPosition(rowcol[0], 0, rowcol[1], 40);
                            // サイズを設定
                            excelImage.SetSize(30, 50);
                        }
                        // イメージを削除
                        if (System.IO.File.Exists(FILE_PATH + stampInfoPlace + ".png"))
                        {
                            // Use a try block to catch IOExceptions, to
                            // handle the case of the file already being
                            // opened by another process.
                            try
                            {
                                System.IO.File.Delete(FILE_PATH + stampInfoPlace + ".png");
                            }
                            catch (System.IO.IOException e)
                            {
                                Console.WriteLine(e.Message);
                                return;
                            }
                        }
                    }
                }

                //save the changes
                excelPackage.Save();

            }
        }

        /// <summary>
        /// スタンプ位置からexcelの位置を変換する。
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal static int[] stampPositionChange(string place)
        {
            string engStr = Regex.Replace(place, "[0-9]", "");
            string numStr = Regex.Replace(place, "[A-Z]", "");
            int[] rowcol = new int[2];
            rowcol[1] = operate26To10Digit(engStr);
            rowcol[0] = int.Parse(numStr) - 1;

            return rowcol;
        }

        /// <summary>
        /// 大文字の英語A-Zの26真数を10真数に変換する。
        /// </summary>
        /// <param name="engStr"></param>
        /// <returns></returns>
        internal static int operate26To10Digit(string engStr)
        {
            double ret = 0;
            int _len = engStr.Length - 1;
            // 大文字のみ取り扱い
            engStr = engStr.ToUpper();
            if ("A".Equals(engStr))
            {
                ret = 0;
            }
            else
            {
                for (int i = 0; i < engStr.Length; i++)
                {
                    int a = System.Char.Parse(engStr.Substring(i, 1)) - 64;
                    double b = Math.Pow(26, (_len - i));
                    ret += a * b;
                }
            }

            return (int)ret - 1;
        }

        /// <summary>
        /// フォルダーパスチェック(テンプレート)
        /// </summary>
        internal static void filePathCheck()
        {
            // パスが存在しないと
            if(!Directory.Exists(FILE_PATH))
            {
                // パスを作成
                Directory.CreateDirectory(FILE_PATH);
            }
        }
        
        /// <summary>
        /// システム日付を取得する。
        /// </summary>
        /// <returns></returns>
        internal static string sysDate()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// NullまたはEmptyの場合true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static bool IsNullOrEmpty(object obj)
        {
            bool flag = false;
            if (obj == null || "".Equals(obj))
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Nullの場合true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static bool IsNull(object obj)
        {
            bool flag = false;
            if (obj == null)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Emptyの場合true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static bool IsEmpty(object obj)
        {
            bool flag = false;
            if ("".Equals(obj))
            {
                flag = true;
            }
            return flag;
        }

        internal static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }
}

