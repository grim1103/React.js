using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hanyokessai.Models
{
    public class Ta100Dto
    {
        public string selTemType { get; set; }

        [Required(ErrorMessage = "テンプレート名を入力してください。")]
        [StringLength(50, ErrorMessage = "テンプレート名は50桁以下に入力してください。")]
        [RegularExpression("[a-z|A-Z|0-9|ぁ-ん|ァ-ン|一-龥|(|)|-|ー|_]*$", ErrorMessage = "テンプレート名には以下の記号は使えません。" + "\\n\r" + ". \" / \\ [ ] : ; | = , ")]
        public string txtTemName { get; set; }

        public string selComName { get; set; }

        public string tmpfile { get; set; }

        public string txtTemAuthor { get; set; }

        public string dateAuthorDate { get; set; }

        [Required(ErrorMessage = "作成者押印位置を入力してください。")]
        [StringLength(11, ErrorMessage = "最終押印位置は10桁以下に入力してください。")]
        public string selApproverStart { get; set; }

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

        public string selLastApprover { get; set; }

        public string txtApprovalMail1 { get; set; }

        public string txtApprovalMail2 { get; set; }

        public string txtApprovalMail3 { get; set; }

        public string txtApprovalMail4 { get; set; }

        public string txtApprovalMail5 { get; set; }

        public string txtApprovalMail6 { get; set; }

        public string txtApprovalMail7 { get; set; }

        public string txtApprovalMail8 { get; set; }

        public string txtApprovalMail9 { get; set; }

        public string txtApprovalMail10 { get; set; }

        public string txtLastApproMail { get; set; }

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

        [StringLength(500, ErrorMessage = "最終押印位置は500桁以下に入力してください。")]
        public string txtRepDetail { get; set; }

        public string txtFileName { get; set; }

        public string txtRegId { get; set; }

        public string txtRegDate { get; set; }

        public string txtDelCd { get; set; }

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

        public List<string> selComNameList { get; set; }

        public List<string> selApproverPosList { get; set; }

        public List<string> selApproverNameList { get; set; }

        public List<string> errorMessege { get; set; }

        public List<string> checkAbleFlg { get; set; }

        public UInt32 tmpFileSize { get; set; }
    }
}