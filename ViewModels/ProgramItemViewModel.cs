using Portal.Services;
/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.ViewModels
{
    public class ProgramItemViewModel
    {
        private Models.SubEvent e;

        public ProgramItemViewModel(Models.SubEvent e)
        {
            Name = e.Title;
            Slug = e.Tag.Id;
            DescriptionShort = (e.Description??"").TruncateHtml(200);
            if (e.Map != null)
            {
                LogoGalleryLow = ThumbnailService.GetThumbnailUrl(e.Map, 110, 110);
                LogoGallery = ThumbnailService.GetThumbnailUrl(e.Map, 542, 362);
                LogoProgram = ThumbnailService.GetThumbnailUrl(e.Map, 450, 150);
            }
        }
        public string Name { get; set; }        

        public string Slug { get; set; }

        public string DescriptionShort { get; set; }

        public string LogoGallery { get; set; }

        public string LogoGalleryLow { get; set; }

        public object LogoProgram { get; set; }
    }

    public class ProgramDetailsViewModel
    {
        public ProgramDetailsViewModel(Models.SubEvent ev)
        {
            Name = ev.Title;
            Description = ev.Description;
            Logo = ThumbnailService.GetThumbnailUrl(ev.Map, 600, 400);
            
            //Galleries = ev.Tag.Galleries.Select(g => new GalleryViewModel(g));            
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public List<GalleryViewModel> Galleries { get; set; }
    }
}
