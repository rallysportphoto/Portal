/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.ViewModels.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Metadata.Models
{
    public class MessageItem
    {
        public MessageItem(MessageViewModel m)
        {
            Title = m.Title;
            Message = m.Message;
        }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}