using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using ESEIM.Models;
using Microsoft.EntityFrameworkCore;
using ESEIM.Utils;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace ESEIM.Utils
{
    public interface ILanguageService
    {
        List<AdLanguage> GetListLanguages();
        AdLanguage GetLanguage(string culture);
        void SetLanguage(AdLanguage language);
        void SetLanguage(string culture);
        JObject GetResource(string culture);
        JObject GetResource();
        void SetResource(string culture);
        void SetResource(JObject resource);

        string GetLanguageDefault();
    }

    public class LanguageService : ILanguageService
    {
        private readonly EIMDBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LanguageService(EIMDBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<AdLanguage> GetListLanguages()
        {
            CommonUtil.Languages = _context.AdLanguages.AsNoTracking().Where(x => x.IsDeleted == false && x.IsEnabled == true).ToList();

            return CommonUtil.Languages;
        }

        public AdLanguage GetLanguage(string culture)
        {
            var language = _context.AdLanguages.AsNoTracking().First(x => x.IsEnabled == true && x.IsDeleted == false && x.Culture.Equals(culture)) ??
                           _context.AdLanguages.AsNoTracking().First(x => x.IsEnabled == true && x.IsDeleted == false && x.IsDefault == true);
            CommonUtil.CurrentLanguage = language;

            if (CommonUtil.Resource == null) SetResource(culture);

            return language;
        }

        public void SetLanguage(AdLanguage language)
        {
            CommonUtil.CurrentLanguage = language;
        }

        public void SetLanguage(string culture)
        {
            var language = _context.AdLanguages.AsNoTracking().First(x => x.IsEnabled == true && x.IsDeleted == false && x.Culture.Equals(culture));
            CommonUtil.CurrentLanguage = language;
        }

        public JObject GetResource(string culture)
        {
            return CommonUtil.Resource;
        }

        public JObject GetResource()
        {
            return CommonUtil.Resource;
        }

        public void SetResource(string culture)
        {
            var resourceObject = new JObject();
            try
            {
                if (string.IsNullOrEmpty(culture))
                    culture = "vi-VN";

                var pathResource = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources/" + culture);

                string[] listFileUrl = Directory.GetFiles(pathResource);

                if (listFileUrl.Length > 0)
                {
                    foreach (var fileUrl in listFileUrl)
                    {
                        if (File.Exists(fileUrl))
                        {
                            var xml = File.ReadAllText(fileUrl);

                            var obj = new
                            {
                                Resouces = XElement.Parse(xml)
                                    .Elements("data")
                                    .Select(el => new
                                    {
                                        Caption = el.Attribute("name").Value,
                                        Value = el.Element("value").Value.Trim()
                                    })
                                    .ToList()
                            };

                            foreach (var lt in obj.Resouces)
                            {
                                resourceObject.Add(lt.Caption, lt.Value);
                            }
                        }
                    }

                    CommonUtil.Resource = resourceObject;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetResource(JObject resource)
        {
            CommonUtil.Resource = resource;
        }

        public string GetLanguageDefault()
        {
            var lang = _context.AdLanguages.FirstOrDefault(x => x.IsDeleted == false && x.IsEnabled && x.IsDefault == true);
            return lang == null ? "vi-VN" : lang.Culture;
        }
    }
}