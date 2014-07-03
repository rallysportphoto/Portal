/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class User : IdentityUser
    {
        [Display(Name="Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Город, страна")]
        public string Location { get; set; }
        [Display(Name = "Контактный телефон")]
        public string Phone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Паспортные данные")]
        public string Passport { get; set; }
        [Display(Name = "Адрес проживания")]
        public string Address { get; set; }
        
        public List<License> Licenses { get; set; }
        public List<Accreditation> Accreditations { get; set; }

    }
}