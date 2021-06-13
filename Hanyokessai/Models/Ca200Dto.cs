using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hanyokessai.Models
{
    public class Ca200Dto
    {
        public string reportID { get; set; }

        public string templeteID { get; set; }

        public string templeteName { get; set; }

        public string reportFile { get; set; }

        public string binaryFile { get; set; }

        public int fileSize { get; set; }

        public string lbComName { get; set; }

        public string lbDep1 { get; set; }

        public string lbDep2 { get; set; }

        public string lbPos { get; set; }

        public string lbAuthorName { get; set; }

        public string lbPlanYmd { get; set; }

        [Required(ErrorMessage = "帳票名を入力してください。")]
        [StringLength(15, ErrorMessage = "50桁以内で入力してください。")]
        public string txtReportName { get; set; }

        public string lbReportName { get; set; }

        [Required(ErrorMessage = "特異事項を入力してください。")]
        [StringLength(15, ErrorMessage = "500桁以内で入力してください。")]
        public string txtReportDetail { get; set; }

        public string lbReportDetail { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail1 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail2 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail3 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail4 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail5 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail6 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail7 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail8 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail9 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtAprvMail10 { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [StringLength(50, ErrorMessage = "50桁以内で入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールは半角文字及び数字を入力してください。")]
        public string txtFnAprvMail { get; set; }

        public string fnAprvNum { get; set; }

        public string fnApprover { get; set; }

        public string fnAprvPlc { get; set; }

        public string fnAprvMail { get; set; }

        public string startAprvMail { get; set; }

        public string startStampImg { get; set; }

        public string stampCd { get; set; }

        public int stampSize { get; set; }

        public string lbAprvNum1 { get; set; }

        public string lbApprover1 { get; set; }

        public string lbAprvPlc1 { get; set; }

        public string lbAprvMail1 { get; set; }

        public string lbAprvNum2 { get; set; }

        public string lbApprover2 { get; set; }

        public string lbAprvPlc2 { get; set; }

        public string lbAprvMail2 { get; set; }

        public string lbAprvNum3 { get; set; }

        public string lbApprover3 { get; set; }

        public string lbAprvPlc3 { get; set; }

        public string lbAprvMail3 { get; set; }

        public string lbAprvNum4 { get; set; }

        public string lbApprover4 { get; set; }

        public string lbAprvPlc4 { get; set; }

        public string lbAprvMail4 { get; set; }

        public string lbAprvNum5 { get; set; }

        public string lbApprover5 { get; set; }

        public string lbAprvPlc5 { get; set; }

        public string lbAprvMail5 { get; set; }

        public string lbAprvNum6 { get; set; }

        public string lbApprover6 { get; set; }

        public string lbAprvPlc6 { get; set; }

        public string lbAprvMail6 { get; set; }

        public string lbAprvNum7 { get; set; }

        public string lbApprover7 { get; set; }

        public string lbAprvPlc7 { get; set; }

        public string lbAprvMail7 { get; set; }

        public string lbAprvNum8 { get; set; }

        public string lbApprover8 { get; set; }

        public string lbAprvPlc8 { get; set; }

        public string lbAprvMail8 { get; set; }

        public string lbAprvNum9 { get; set; }

        public string lbApprover9 { get; set; }

        public string lbAprvPlc9 { get; set; }

        public string lbAprvMail9 { get; set; }

        public string lbAprvNum10 { get; set; }

        public string lbApprover10 { get; set; }

        public string lbAprvPlc10 { get; set; }

        public string lbAprvMail10 { get; set; }

        public string regID { get; set; }

        public string regDate { get; set; }

        public string updID { get; set; }

        public string updDate { get; set; }

        public string regYm { get; set; }

        public string delCd { get; set; }

        public string status { get; set; }

        public bool rejectFlg { get; set; }

        public string befReportID { get; set; }
    }
}

