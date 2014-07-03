/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        public string LoginProvider { get; set; }

        [Display(Name = "Фамилия"), Required]
        public string LastName { get; set; }
        [Display(Name = "Имя"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Город, Страна")]
        public string Location { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Паспортные данные")]
        public string Passport { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен состоять не менее, чем из {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле {0} обязательно к заполнению")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле {0} обязательно к заполнению")]
        [StringLength(100, ErrorMessage = "{0} должен состоять не менее, чем из {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле {0} обязательно к заполнению")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        //[Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле {0} обязательно к заполнению")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Поле {0} обязательно к заполнению")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Город, Страна")]
        public string Location { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Паспортные данные")]
        public string Passport { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Лицензия водителя")]
        public string DriverLicense { get; set; }
        [Display(Name = "Лицензия судьи")]
        public string MarchalLicense { get; set; }
    }
}