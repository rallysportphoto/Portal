/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Services
{
    public class DocumentService
    {
        public static string GetFileRelativePath(string fileName)
        {
            return "~/App_Data/Files/" + fileName;
        }



        internal static bool IsFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }

        internal static void WriteFile(string path, HttpPostedFileBase content)
        {
            content.SaveAs(path);
        }

       
        internal static void RemoveFile(string path)
        {
            if (IsFileExist(path))
                System.IO.File.Delete(path);
        }



        internal static File CreateAutomaticFile(HttpPostedFileBase file, DateTime? date=null, string description="", string title="", bool hidden=false)
        {
            var path = HttpContext.Current.Server.MapPath(DocumentService.GetFileRelativePath(file.FileName));
            DocumentService.WriteFile(path, file);

            return new File()
                        {
                            Date = date ?? DateTime.Now,
                            Description = description,
                            FileName = file.FileName,
                            Title = title,
                            Hidden = hidden
                        };
        }
    }
}