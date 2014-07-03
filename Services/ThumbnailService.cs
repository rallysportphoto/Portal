/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Portal.Services
{
    public class ThumbnailService
    {
        public static string GetThumbnailUrl(Models.File file, int width, int height)
        {
            if (file == null)
            {
                return "/images/default.png";
            }
            return "/Thumbnail/" + width + "/" + height + "/" + file.FileName;
        }

        internal static string GetThumbnailPath(string name, int width, int height)
        {
            return "~/App_Data/Thumbnails/" + width + "_" + height + "_" + name;
        }

        internal static string GenerateThumbnail(Models.File file, int width, int height)
        {
            var path = HttpContext.Current.Server.MapPath(DocumentService.GetFileRelativePath(file.FileName));
            var thumbnailUrl = GetThumbnailUrl(file,width,height);
            var thumbPath = HttpContext.Current.Server.MapPath(GetThumbnailPath(file.FileName,width,height)); 

            Image image = Image.FromFile(path);

            Image thumb = Crop(image, width, height, AnchorPosition.Center);
            thumb.Save(thumbPath);
            return thumbPath;
        }

        enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }
        static Image Crop(Image imgPhoto, int Width,
                    int Height, AnchorPosition Anchor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)
                            (Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)
                            ((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)
                          (Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)
                          ((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width,
                    Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        internal static string GetRawFile(Models.File file)
        {
            if (file != null)
                return "/file/" + file.FileName;
            return "/images/default.png";

        }

        internal static void ValidateCache(Models.File file)
        {
            if (file.Type == Models.FileType.Image)
            {
                foreach(var f in  System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/App_Data/Thumbnails/"),"*"+ file.FileName)) {
                    System.IO.File.Delete(f);
                }
            }
        }
    }
}