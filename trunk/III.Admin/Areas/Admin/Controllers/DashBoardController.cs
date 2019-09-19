using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using III.Admin.Controllers;

namespace ESEIM.Controllers
{
    [Area("Admin")]
    public class DashBoardController : BaseController
	{
		private readonly UserManager<AspNetUser> _userManager;
		private readonly RoleManager<AspNetRole> _roleManager;
		private readonly EIMDBContext _context;
		private readonly AppSettings _appSettings;
		private readonly ILogger _logger;
		private readonly IHostingEnvironment _hostingEnvironment;
		public class JTableModelCustom : JTableModel
		{

			public string Code { get; set; }
			public string Title { get; set; }


		}

		public class JMessage2 : JMessage
		{
			public object Object2 { get; set; }
		}
		public DashBoardController(IOptions<AppSettings> appSettings, EIMDBContext context, UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager, IHostingEnvironment hostingEnvironment)
		{
			_userManager = userManager;
			_context = context;
			_roleManager = roleManager;
			_appSettings = appSettings.Value;
			_hostingEnvironment = hostingEnvironment;
		}

		public IActionResult Index()
        {
			return View();
		}
        
    }
}