using System.ComponentModel.DataAnnotations;

namespace Hanyokessai.Models

{
    public class Za500Dto
    {
        [Required(ErrorMessage = "パスワードを入力してください。")]
        [StringLength(15, ErrorMessage = "15桁以内で入力してください。")]
        [RegularExpression(@"^[a-zA-Z0-9!-/:-@¥[-`{-~]*$", ErrorMessage = "半角文字及び数字を入力してください。")]
        public string password { get; set; }
        public string email { get; set; }
    }
}