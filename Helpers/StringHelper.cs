/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Portal.Helpers
{
    public static class StringHelper
    {
        public static string OrDefault(this string str)
        {
            return str ?? "";
        }

        public static string ToMinutes(this decimal timeValue)
        {            
           return ToMinutes(TimeSpan.FromSeconds((double)timeValue));
        }

        public static string ToMinutes(this TimeSpan timespan)
        {            

            StringBuilder builder = new StringBuilder();

            if (timespan.Hours > 0)
            {
                builder.Append(Math.Abs(timespan.Hours));
                builder.Append(':');
                builder.Append(Math.Abs(timespan.Minutes).ToString("00"));
                builder.Append(':');
                builder.Append(Math.Abs(timespan.Seconds).ToString("00"));

            }
            else if (timespan.Minutes > 0)
            {
                builder.Append(Math.Abs(timespan.Minutes));
                builder.Append(':');
                builder.Append(Math.Abs(timespan.Seconds).ToString("00"));
            }
            else
            {
                builder.Append(Math.Abs(timespan.Seconds));
            }
            builder.Append('.');
            builder.Append(Math.Abs((int)(timespan.Milliseconds / 100)));
            return builder.ToString();
        }
    }    
}