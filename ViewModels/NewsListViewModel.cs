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

    public class NewsListViewModel
    {
        public NewsListViewModel(News item)
        {
            Title = item.Title;
            Date = item.Date;
            Brief = item.Body.TruncateHtml(100, "...");
            Slug = item.Slug;

            if (item.Attachments.Any(a => a.Type == FileType.Image))
            {
                LogoUrl = ThumbnailService.GetThumbnailUrl(item.Attachments.First(f => f.Type == FileType.Image), 100, 100);
                LogoUrlWide = ThumbnailService.GetThumbnailUrl(item.Attachments.First(f => f.Type == FileType.Image), 320, 200);
            }
            else LogoUrl = "/images/default.png";
            Type = item.Type;
            switch (item.Type)
            {
                case NewsType.Gallery:
                    LogoClass = "fa-picture-o";
                    break;
                case NewsType.Quote:
                    LogoClass ="";
                    break;
                case NewsType.Video:
                    LogoClass ="fa-film";
                    break;
                default:
                    LogoClass = "fa-pencil";
                    break;
            
            }
        }

        public string LogoUrl { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Brief { get; set; }

        public string LogoClass { get; set; }

        public object Slug { get; set; }

        public NewsType Type { get; set; }



        public string LogoUrlWide { get; set; }
    }
}