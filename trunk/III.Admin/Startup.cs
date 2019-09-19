using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using ESEIM.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using ESEIM.Utils;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace ESEIM
{
    public class Startup
    {
        //private const string enUSCulture = "en-US";
        //private const string viVNCulture = "vi-VN";
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();

            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);
            services.AddDbContext<EIMDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("EIMConnection"), opt => opt.UseRowNumberForPaging());
            });
            services.AddIdentity<AspNetUser, AspNetRole>(options =>
            options.Password = new PasswordOptions
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonAlphanumeric = false
            }).AddEntityFrameworkStores<EIMDBContext>().AddDefaultTokenProviders();
            services.AddResponseCaching();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                //options.SerializerSettings.DateFormatString = "MM/dd/yyyy";
            });
            //services.AddMvc()
            //    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            //    .AddDataAnnotationsLocalization();
            services.AddSession();
            services.AddCors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IActionLogService, ActionLogService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IFCMPushNotification, FCMPushNotification>();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Environment.ContentRootPath + "\\TempKeys"));
            // Add AutoMapper config
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile<AutoMapperProfile>();
            //});

            ///<summary>
            /// Kết nối vào SSO
            /// </summary>
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddOpenIdConnect(options =>
            //{
            //    options.SignInScheme = "Cookies";
            //    options.Authority = appSettings.GetValue<string>("Authority");
            //    options.SignedOutRedirectUri = appSettings.GetValue<string>("PostLogoutRedirectUri");

            //    options.RequireHttpsMetadata = false;
            //    options.ClientId = appSettings.GetValue<string>("ClientId");
            //    options.ClientSecret = appSettings.GetValue<string>("ClientSecret");

            //    options.ResponseType = "code id_token";
            //    //options.Scope ={ "profile", "email" };
            //    options.RefreshOnIssuerKeyNotFound = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.SaveTokens = true;
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //    options.UseTokenLifetime = false;
            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnRemoteFailure = context =>
            //        {
            //            if (context.Failure.Message.Contains("Correlation failed"))
            //                context.Response.Redirect("/");
            //            else
            //                context.Response.Redirect("/");
            //            context.HandleResponse();
            //            return Task.CompletedTask;
            //        },
            //    };
            //});
            ///<summary>
            /// Kết nối vào SSO
            /// </summary>
            #region Config Session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // Địa chỉ trả về khi chưa đăng nhập /Admin/Account/Login.
                options.LoginPath = "/Admin/Account/Login";
                // Địa chỉ trả về khi không cho phép truy cập /Admin/Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });
            services.AddAuthentication("LoginSecurityScheme")
                .AddCookie("LoginSecurityScheme", options =>
                {
                    options.AccessDeniedPath = new PathString("/Account/Access");
                    options.Cookie = new CookieBuilder
                    {
                        //Domain = "",
                        HttpOnly = true,
                        Name = ".swork.Security.Cookie",
                        Path = "/",
                        SameSite = SameSiteMode.Lax,
                        SecurePolicy = CookieSecurePolicy.SameAsRequest
                    };
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnSignedIn = context =>
                        {
                            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                                "OnSignedIn", context.Principal.Identity.Name);
                            return Task.CompletedTask;
                        },
                        OnSigningOut = context =>
                        {
                            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                                "OnSigningOut", context.HttpContext.User.Identity.Name);
                            return Task.CompletedTask;
                        },
                        OnValidatePrincipal = context =>
                        {
                            Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                                "OnValidatePrincipal", context.Principal.Identity.Name);
                            return Task.CompletedTask;
                        }
                    };
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = new PathString("/Admin/Account/Login");
                    options.ReturnUrlParameter = "returnUrl";
                });

            //services.Configure<SecurityStampValidatorOptions>(options =>
            //options.ValidationInterval = TimeSpan.FromDays(30));
            #endregion


            //#region MutiLanguage
            //services.AddLocalization(options => options.ResourcesPath = "Resources");



            //Configure supported cultures and localization options
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //      new CultureInfo(enUSCulture),
            //      new CultureInfo(viVNCulture)
            //    };

            //    options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});
            //#endregion

        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IParameterService parameterService)
        {
            var appSettings = Configuration.GetSection("AppSettings");
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddContext(LogLevel.Information, app.ApplicationServices.GetService<IHttpContextAccessor>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Admmin/DashBoard/Index");
            }

            AppContext.Configure(app.ApplicationServices.
                GetRequiredService<IHttpContextAccessor>(), appSettings
            );

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "Cookies",
            //    DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(Environment.ContentRootPath + "\\temp-keys")),
            //});
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseResponseCaching();
            app.UseSession();
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            #region MutiLanguage
            //var supportedCultures = new[]
            //        {
            //            new CultureInfo(enUSCulture),
            //            new CultureInfo(viVNCulture),
            //            new CultureInfo("en-AU"),
            //            new CultureInfo("en-GB"),
            //            new CultureInfo("en"),
            //            new CultureInfo("es-ES"),
            //            new CultureInfo("es-MX"),
            //            new CultureInfo("es"),
            //            new CultureInfo("fr-FR"),
            //            new CultureInfo("fr"),
            //        };

            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture(enUSCulture),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures
            //});

            app.UseStaticFiles();
            // To configure external authentication, 
            // see: http://go.microsoft.com/fwlink/?LinkID=532715
            //app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();
            #endregion
            //app.UseRequestLocalization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=CalendarRegistration}/{action=Index}");
                routes.MapRoute(
                    name: "admin",
                    template: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");
                routes.MapRoute(
                 name: "angular",
                 template: "{*url}",
                 defaults: new { controller = "CalendarRegistration", action = "Index" });
            });
        }
    }

    public class AppSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Host { get; set; }
        public string Api { get; set; }
        public string ClientScope { get; set; }
        public ADConfigs ADConfigs { get; set; }
        public OMSConfigs OMSConfigs { get; set; }
    }

    public class ADConfigs
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string LoginDN { get; set; }
        public string Password { get; set; }
        public string ObjectDN { get; set; }
    }

    public class OMSConfigs
    {
        public string Url { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
