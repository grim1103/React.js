using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hanyokessai.Models
{
    public class Zb300Dto
    {
        public string member_id { get; set; }

        public string txtAuthorityCode { get; set; }
        
        public string txtUpdateid { get; set; }

        public string txtMail { get; set; }

        public string txtPwd { get; set; }

        [StringLength(30, ErrorMessage = "姓(カナ)に30桁以内で入力してください。")]
        [RegularExpression("^[ｦ-ﾟ]*$", ErrorMessage = "姓(カナ)に半角カナを入力してください。")]
        public string txtKanaFirstName { get; set; }

        [StringLength(30, ErrorMessage = "名(カナ)に30桁以内で入力してください。")]
        [RegularExpression("^[ｦ-ﾟ]*$", ErrorMessage = "名(カナ)に半角カナを入力してください。")]
        public string txtKanaLastName { get; set; }

        [Required(ErrorMessage = "姓(漢字)を入力してください。")]
        [StringLength(15, ErrorMessage = "姓(漢字)を15桁以内で入力してください。")]
        [RegularExpression("^[^-~｡-ﾟ]*$", ErrorMessage = "姓(漢字)に全角文字を入力してください。")]
        public string txtKanjiFirstName { get; set; }

        [Required(ErrorMessage = "名(漢字)を入力してください。")]
        [StringLength(15, ErrorMessage = "名(漢字)に15桁以内で入力してください。")]
        [RegularExpression("^[^-~｡-ﾟ]*$", ErrorMessage = "名(漢字)に全角文字を入力してください。")]
        public string txtKanjiLastName { get; set; }

        [StringLength(30, ErrorMessage = "姓(英語)に30桁以内で入力してください。")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "姓(英語)に半角英語を入力してください。")]
        public string txtEngFirstName { get; set; }

        [StringLength(30, ErrorMessage = "名(英語)に30桁以内で入力してください。")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "名(英語)に半角英語を入力してください。")]
        public string txtEngLastName { get; set; }

        public string txtGender { get; set; }

        [RegularExpression("^[0-9]{3}[-][0-9]{4}[-][0-9]{4}$", ErrorMessage = "携帯番号の形式ではありません。")]
        public string txtPhone { get; set; }

        [RegularExpression("^[0-9]{3}[-][0-9]{4}$", ErrorMessage = "郵便番号の形式ではありません。")]
        public string txtZipcode { get; set; }

        [StringLength(75, ErrorMessage = "住所に75桁以内で入力してください。")]
        [RegularExpression("^[^-~｡-ﾟ]*$", ErrorMessage = "住所に全角文字を入力してください。")]
        public string txtAddress { get; set; }

        public string txtCompanyCode { get; set; }

        public string txtDepartment1 { get; set; }

        public string txtDepartment2 { get; set; }

        public string txtPosition { get; set; }

        public string txtEnterDate { get; set; }

        public string txtUpdateDate { get; set; }

        public string txtimgStamp { get; set; }

        public Boolean stampFlg { get; set; }

        public string txtStampCode { get; set; }

        public List<string> errorMessege { get; set; }

        public List<string> txtDep1CdList { get; set; }

        public List<string> txtDep1NameList { get; set; }

        public List<string> txtDep2CdList { get; set; }

        public List<string> txtDep2CdNameList { get; set; }

        public List<string> txtComPosCdList { get; set; }

        public List<string> txtComPosCdNameList { get; set; }


    }
}
