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
    public class GalleryViewModel
    {
        public GalleryViewModel(Models.Gallery g)
        {
            Name = g.Name;
            Slug = g.Slug;
            Description = g.Author;
            Code = g.Code;
            if (g.Files.Any(f => f.Type == FileType.Image))
            {
                var logo = g.Files.OrderBy(f=>Guid.NewGuid()).First();

                LogoThumbnail = ThumbnailService.GetThumbnailUrl(logo, 600, 400);
                LogoUrl = ThumbnailService.GetRawFile(logo);                
            }
            else
            {
                LogoUrl = LogoThumbnail = "/images/default.png";                
            }

            Images = g.Files.Select(f => new FileViewModel(f, true)).ToList();

        }

        public string LogoThumbnail { get; set; }

        public string LogoUrl { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public List<FileViewModel> Images { get; set; }

        public string Code { get; set; }
    }
}