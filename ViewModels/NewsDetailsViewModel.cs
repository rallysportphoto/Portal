/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class NewsDetailsViewModel
    {
        public NewsDetailsViewModel(News item) {
            Title = item.Title;
            Date = item.Date;
            if (item.Attachments.Any(f => f.Type == FileType.Image))
            {
                var logo = item.Attachments.First(f => f.Type == FileType.Image);
                LogoThumbnail = ThumbnailService.GetThumbnailUrl(logo, 600, 400);
                LogoUrl = ThumbnailService.GetRawFile(logo);
                Attachments = item.Attachments.Where(f=>f!=logo).Select(f => new FileViewModel(f)).ToList();
            }
            else
            {
                LogoUrl = LogoThumbnail = "/images/default.png";
                Attachments = new List<FileViewModel>();
            }
            Body = item.Body;
            
        }

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string LogoUrl { get; set; }
        public string Body { get; set; }

        public string LogoThumbnail { get; set; }

        public List<FileViewModel> Attachments { get; set; }
    }
}