using System;
using System.IO;
using Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public static class UploadImage
    {
        public static IWebHostEnvironment _environment { get; set; }
        public static string upload(IFormFile objeFile, string fileName)
        {
            try
            {
                if (objeFile.Length > 0)
                {
                    string paste = "\\App_Data\\";

                    string imagePath = _environment.WebRootPath;

                    if (objeFile.FileName.Contains(".jpg"))
                        fileName += ".jpg";
                    else if (objeFile.FileName.Contains(".gif"))
                        fileName += ".gif";
                    else if (objeFile.FileName.Contains(".png"))
                        fileName += ".png";

                    //verify if directory exist
                    if (!Directory.Exists(_environment.WebRootPath + paste))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + paste);
                        imagePath += paste + fileName;
                    }
                    else
                    {
                        imagePath += paste + fileName;
                    }

                    using (FileStream fileStream = System.IO.File.Create(imagePath))
                    {
                        objeFile.CopyTo(fileStream);
                        fileStream.Flush();
                        return imagePath;
                    }
                }
                return "Failed";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
    }
}