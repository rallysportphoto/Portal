/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Services
{
    public class SettingsService
    {
        public static string ServiceEmail
        {
            get
            {
                return "noreply@afspb.org.ru";
            }
        }

        public static string OrganisationEmail
        {
            get
            {
                return "afspb@list.ru";
            }
        }

        public static string SiteName
        {
            get
            {
                return "http://afspb.org.ru/";
            }
        }
    }
}