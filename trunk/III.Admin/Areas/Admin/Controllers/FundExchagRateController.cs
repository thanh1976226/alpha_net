using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class FundExchagRateController : BaseController
    {
        public class FundExchagRatesJtableModel
        {
            public int Id { get; set; }
            public string Currency { get; set; }
            public decimal Rate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedTime { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedTime { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedTime { get; set; }
            public bool IsDeleted { get; set; }
        }
        private readonly EIMDBContext _context;

        public FundExchagRateController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelAct : JTableModel
        {

            public int Id { get; set; }
            public string Currency { get; set; }
            public decimal Rate { get; set; }
        }

        #region action

        //[HttpPost]
        public async Task<object> JTable([FromBody]JTableModelAct jTablePara)
        {

            var urlChange = "https://api.exchangeratesapi.io/latest?base=USD";

            var obj = await CommonUtil.SendAPIRequest(urlChange);


            JObject jObject = JObject.Parse(obj.Object.ToString());
            JToken rate = jObject["rates"];

            var listChangeRate = new List<ChangeRate>();
            foreach (var item in rate)
            {
                var key = ((Newtonsoft.Json.Linq.JProperty)item).Name;
                var value = ((Newtonsoft.Json.Linq.JProperty)item).Value.ToString();

                var objRate = new ChangeRate
                {
                    Key = key,
                    Value = value,
               
                };
                listChangeRate.Add(objRate);
            }

            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in listChangeRate
                        where (string.IsNullOrEmpty(jTablePara.Currency) || a.Key.ToLower().Equals(jTablePara.Currency.ToLower()))
                        select new FundExchagRatesJtableModel
                        {
                            Currency = a.Key,
                            Rate = decimal.Parse(a.Value)
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "Currency", "Rate");
            return Json(jdata);

        }


        public class ChangeRate
        {
            public string Key { set; get; }
            public string Value { set; get; }
        }
        #endregion
    }
}