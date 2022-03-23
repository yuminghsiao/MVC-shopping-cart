using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CARTS.Models
{
    public class ManageUser
    {
        [Required]
        public String Id { get; set; }

        [Required]//必要欄位
        [StringLength(256, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 1)] //字元長度1~256
        [Display(Name = "暱稱")]//欄位顯示文字
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
    }
}