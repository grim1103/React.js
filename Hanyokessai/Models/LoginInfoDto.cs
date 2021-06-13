using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hanyokessai.Models
{
    public class LoginInfoDto
    {
        public string txtId { get; set; }

        [Required(ErrorMessage = "メールを入力してください。")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "メールにて誤りがあります。（例：XXXX@XXXXX.XXX）")]
        public string txtMail { get; set; }

        [Required(ErrorMessage = "パスワードを入力してください。")]
        public string txtPwd { get; set; }

        public List<string> errorMessege { get; set; }

        public string txtAuthorityCd { get; set; }

        public string txtCompanyCd { get; set; }
    }
}