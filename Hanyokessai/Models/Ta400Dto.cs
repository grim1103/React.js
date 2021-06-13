using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hanyokessai.Models
{
    public class Ta400Dto
    {
        public string auth { get; set; }

        public string temp_id { get; set; }
        public string txtTemId { get; set; }

        public string txtTemName { get; set; }

        public string txtRepDetail { get; set; }

        [Required(ErrorMessage = "ファイルを入力してください。")]
        [StringLength(50, ErrorMessage = "ファイル名は50桁以下に入力してください。")]
        [RegularExpression("[a-z|A-Z|0-9|ぁ-ん|ァ-ン|一-龥|(|)|-|_]*$", ErrorMessage = "ファイル名には以下の記号は使えません。" + "\\n" + ". \" / \\ [ ] : ; | = , ")]
        public string txtTemFileName { get; set; }

        public string file_binary { get; set; }

        public string temp_filesize { get; set; }

        [Required(ErrorMessage = "作成者押印位置を入力してください。")]
        [StringLength(11, ErrorMessage = "最終押印位置は10桁以下に入力してください。")]
        public string txtStartApproPlace { get; set; }

        public string txtApproPlace1 { get; set; }

        public string txtApproPlace2 { get; set; }
        
        public string txtApproPlace3 { get; set; }
        
        public string txtApproPlace4 { get; set; }
        
        public string txtApproPlace5 { get; set; }
        
        public string txtApproPlace6 { get; set; }

        public string txtApproPlace7 { get; set; }

        public string txtApproPlace8 { get; set; }

        public string txtApproPlace9 { get; set; }

        public string txtApproPlace10 { get; set; }

        [Required(ErrorMessage = "最終押印位置を入力してください。")]
        [StringLength(11, ErrorMessage = "最終押印位置は10桁以下に入力してください。")]
        [RegularExpression("^[A-Za-z0-9+]*$", ErrorMessage = "最終押印位置は記号使えません。" + "\\n" + ". \" / \\ [ ] : ; | = , ")]
        public string txtLastApproPlace { get; set; }

        public string txtApproMail1 { get; set; }
        
        public string txtApproMail2 { get; set; }
        
        public string txtApproMail3 { get; set; }
        
        public string txtApproMail4 { get; set; }
        
        public string txtApproMail5 { get; set; }
        
        public string txtApproMail6 { get; set; }
        
        public string txtApproMail7 { get; set; }
        
        public string txtApproMail8 { get; set; }
        
        public string txtApproMail9 { get; set; }

        public string txtApproMail10 { get; set; }

        public string txtLastApproMail { get; set; }

        public string selApprover1 { get; set; }

        public string selApprover2 { get; set; }

        public string selApprover3 { get; set; }

        public string selApprover4 { get; set; }

        public string selApprover5 { get; set; }

        public string selApprover6 { get; set; }

        public string selApprover7 { get; set; }

        public string selApprover8 { get; set; }

        public string selApprover9 { get; set; }

        public string selApprover10 { get; set; }

        [Required(ErrorMessage = "最終決裁者を入力してください。")]
        public string selLastApprover { get; set; }

        public string reportFile { get; set; }

        public uint fileSize { get; set; }

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "変更者に半角英語を入力してください。")]
        [StringLength(10, ErrorMessage = "10桁以内で入力してください。")]
        public string txtRenewalrName { get; set; }

        public string txtRenewalrDate { get; set; }

        public string checkAbleFlg1 { get; set; }

        public string checkAbleFlg2 { get; set; }

        public string checkAbleFlg3 { get; set; }

        public string checkAbleFlg4 { get; set; }

        public string checkAbleFlg5 { get; set; }

        public string checkAbleFlg6 { get; set; }

        public string checkAbleFlg7 { get; set; }

        public string checkAbleFlg8 { get; set; }

        public string checkAbleFlg9 { get; set; }

        public string checkAbleFlg10 { get; set; }

        public List<string> selApproverPosList { get; set; }

        public List<string> selApproverNameList { get; set; }

        public List<string> errorMessege { get; set; }
    }
}