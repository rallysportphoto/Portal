/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels.Translation
{
    public class MessageViewModel
    {        
        public MessageViewModel(TrackMePro.MessageViewModel m)
        {
            Title = m.Time.ToString("HH:mm") + " " + m.Title;
            Message = m.Message;
            Time = m.Time;
        }

        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}