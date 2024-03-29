﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Extension
{
    public class Saveimage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Saveimage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadImage(string folderPath, IFormFile file, string fileName)
        {
            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt.ToLower())) // Khác các file định nghĩa
            {
                return null;
            }
            string extension = Path.GetExtension(file.FileName);

            folderPath += Utilities.SEOUrl(fileName) + "_preview_" + Guid.NewGuid() + extension;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
