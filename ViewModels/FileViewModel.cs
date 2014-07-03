/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.ViewModels
{
    public class FileViewModel 
    {
        
        public FileViewModel(File f)
        {
            Title = f.Title;
            //Logo = TODO
            FileName = f.FileName;
            PublicationDate = f.Date;
        }

        public FileViewModel(File f, bool use_services)
        {
            Title = f.Title;
            //Logo = TODO            
            PublicationDate = f.Date;

            Logo = ThumbnailService.GetThumbnailUrl(f, Properties.Settings.Default.ImageThumbnailWidth, Properties.Settings.Default.ImageThumbnailHeight);
            FileName = ThumbnailService.GetRawFile(f);                
        }

        public string Title { get; set; }
        public string Logo { get; set; }
        public string FileName { get; set; }

        public DateTime PublicationDate { get; set; }

       
    }
}
