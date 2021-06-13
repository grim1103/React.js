using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hanyokessai
{
    class Constants
    {
        public class View
        {
            // ログイン画面
            public const String ZA200 = "Za200";
            // 会員加入
            public const String ZA300 = "Za300";
            // 会員情報修正
            public const String ZA400 = "Za400";
            // 会員脱会
            public const String ZA500 = "Za500";
            // 会員情報照会
            public const String ZA600 = "Za600";
            // ユーザ情報管理一覧
            public const String ZB100 = "Zb100";
            // ユーザ情報管理照会
            public const String ZB200 = "Zb200";
            // ユーザ情報管理修正
            public const String ZB300 = "Zb300";
            // 押印グループ管理
            public const String ZB400 = "Zb400";
            // メイン 
            public const String ZA100 = "Za100";
            // テンプレート登録
            public const String TA100 = "Ta100";
            // テンプレート一覧
            public const String TA200 = "Ta200";
            // テンプレート照会
            public const String TA300 = "Ta300";
            // テンプレート修正
            public const String TA400 = "Ta400";
            // 帳票作成
            public const String CA100 = "Ca100";
            // 帳票修正c
            public const String CA200 = "Ca200";
            // 帳票一覧
            public const String CA300 = "Ca300";
            // 帳票照会
            public const String CA400 = "Ca400";
            // 決裁管理＿全体
            public const String KA100 = "Ka100";
            // 決裁管理一覧
            public const String KA200 = "Ka200";
            // 決裁対象照会
            public const String KA500 = "ka500";
        }

        public class Position
        {
            public static readonly Dictionary<String, String> GetPosition = new Dictionary<string, string>()
            {
                {"CEO","会長"},
                {"COO","社長"},
                {"EM","本部長"},
                {"AEM","本部長代理"},
                {"GM","部長"},
                {"AGM","部長代理"},
                {"MG","マネージャー"},
                {"AMG","マネージャー代理"},
                {"SLD","総括リーダー"},
                {"LD","リーダー"},
                {"MB","メンバー"},
                {"BP","ビジネスパートナー"}
            };
        }

        public class Department
        {
            public static readonly Dictionary<String, String> GetDepartment1 = new Dictionary<string, string>()
            {
                {"1000000000","開発本部"},
                {"2000000000","人事本部"},
                {"3000000000","営業本部"},
                {"4000000000","管理本部"},
                {"5000000000","アウトソーシング事業部"},
                {"9000000000","秘書室"}
            };

            public static readonly Dictionary<String, String> GetDepartment2 = new Dictionary<string, string>()
            {
                {"1000000100","開発１部"},
                {"1000000200","開発２部"},
                {"1000000300","開発３部"},
                {"1000000400","開発４部"},
                {"1000000500","開発５部"},
                {"1000000600","開発６部"},
                {"2000000100","人事部"},
                {"2000000200","対内人事部"},
                {"2000000300","対外人事部"},
                {"3000000100","営業１部"},
                {"3000000101","営業２部"},
                {"3000000102","営業３部"},
                {"3000000103","営業４部"},
                {"4000000100","管理部"},
                {"5000000100","経理"},
                {"5000000200","法務"},
                {"9000000100","秘書室１"}
            };
        }
  
    }
}
