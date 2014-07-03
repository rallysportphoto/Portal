/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace Portal.Services
{
    public class EmailService
    {
        public static void SendAdministrativeMail(string title, string body) {
            var msg = new MailMessage()
            {
                Subject = title,
                Body = body,
                IsBodyHtml = false                
            };

            msg.From = new MailAddress(SettingsService.ServiceEmail);
            msg.To.Add(new MailAddress(SettingsService.OrganisationEmail));

            var c = new SmtpClient();
            c.Send(msg);
        }

        internal static void SendPasswordResetMail(Models.User user, string pwd)
        {
            var body = string.Format(@"<html><body><h3>Уважаемый пользователь, </h3>
                        <br/>
                         <p>Ваши учетные данные были обновлены на сайте {0}.<br/>
                         Новые учетные данные: имя пользователя {1}, пароль {2}<br/><br/>

                         Администрация сайта {0}</p></body></html>",SettingsService.SiteName,user.UserName, pwd);

            var msg = new MailMessage() {
                Subject = "Смена учетных данных на сайте " + SettingsService.SiteName,
                Body = body,
                IsBodyHtml = true
            };

            msg.From = new MailAddress(SettingsService.ServiceEmail);
            msg.To.Add(new MailAddress(user.Email));

            var c = new SmtpClient();
            c.Send(msg);
        }
    }
}