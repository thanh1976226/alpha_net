using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESEIM.Utils
{
    public interface IUploadService
    {
        JMessage UploadFile(IFormFile fileUpload,string pathUpload);
        JMessage UploadImage(IFormFile FileUpload);
    }

    public class UploadService : IUploadService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public JMessage UploadFile(IFormFile fileUpload, string pathUpload)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var filePath = Path.GetTempFileName();
                if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);
                var fileName = Path.GetFileName(fileUpload.FileName);
                fileName = Path.GetFileNameWithoutExtension(fileName)
                          + "_"
                          + Guid.NewGuid().ToString().Substring(0, 8)
                          + Path.GetExtension(fileName);
                var fullPath = Path.Combine(pathUpload, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    fileUpload.CopyTo(stream);
                }
                mess.Object = fileName;
                //fileUpload.CopyTo(new FileStream(fullPath, FileMode.Create));
            }
            catch (Exception ex)
            {
                mess.Error = true;
                mess.Title = ex.Message;
            }
            return mess;
        }
        public JMessage UploadImage(IFormFile fileUpload)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var filePath = Path.GetTempFileName();
                var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\images");
                if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);
                var fileName = Path.GetFileName(fileUpload.FileName);
                fileName = Path.GetFileNameWithoutExtension(fileName)
                          + "_"
                          + Guid.NewGuid().ToString().Substring(0, 8)
                          + Path.GetExtension(fileName);
                var fullPath = Path.Combine(pathUpload, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    fileUpload.CopyTo(stream);
                }
                mess.Object = fileName;
                //fileUpload.CopyTo(new FileStream(fullPath, FileMode.Create));
            }
            catch (Exception ex)
            {
                mess.Error = true;
                mess.Title = ex.Message;
            }
            return mess;
        }
    }
    public class JmessageExtension: JMessage
    {
        public string FileName { get; set; }
    }
}
