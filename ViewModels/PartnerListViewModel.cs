/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class PartnerListViewModel
    {
        public PartnerListViewModel(Models.Partner p)
        {
            Position = p.Position;
            Title = p.Desctiption;
            Name = p.Name;
            Url = p.Url;
            Global = p.Global;
            File = ThumbnailService.GetRawFile(p.Logo);

            if (p.Logo != null)
            {
                Logo = ThumbnailService.GetThumbnailUrl(p.Logo, Properties.Settings.Default.LogoBigThumbnailWidth, Properties.Settings.Default.LogoBigThumbnailHeight);
                LogoLow = ThumbnailService.GetThumbnailUrl(p.Logo, Properties.Settings.Default.LogoSmallThumbnailWidth, Properties.Settings.Default.LogoSmallThumbnailWidth);                
            }
            else
            {
                Logo = "/images/default.png";
                LogoLow = "/images/default.png";
            }
        }

        public bool Global { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public string LogoLow { get; set; }
        public string File { get; set; }
        public string Name { get; set; }
    }
}