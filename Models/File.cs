/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Hidden { get; set; }

        public FileType Type
        {
            get
            {
                if (FileName == null) return FileType.Other;
                var fileNameLowered = FileName.ToLower();
                if (fileNameLowered.EndsWith(".png") || fileNameLowered.EndsWith(".jpg") || fileNameLowered.EndsWith(".jpeg") || fileNameLowered.EndsWith(".gif") || fileNameLowered.EndsWith(".bmp"))
                    return FileType.Image;
                else
                    return FileType.Other;                
            }
        }

        public List<Tag> Tags { get; set; }

        public DateTime Date { get; set; }
    }

    public enum FileType
    {
        Image,        
        Other
    }
}