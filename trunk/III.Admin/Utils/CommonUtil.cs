using System;
using System.Collections.Generic;
using System.Linq;
using ESEIM.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MimeKit;
using MailKit.Net.Smtp;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using MailKit.Security;
using QRCoder;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace ESEIM.Utils
{
    public static class CommonUtil
    {
        private static string sqlConnectString = "server=117.6.131.222;database=SmartEducation;uid=user_smart_edu;password=Vietnam@3i;multipleactiveresultsets=true";
        private static SqlConnection sqlConn = new SqlConnection(sqlConnectString);

        private static string GoogleDistanceMatrixKey = "AIzaSyCCNEyZzoebk3a7IT4HZth3j1CLR-PXEgg";
        public static List<AdLanguage> Languages { get; set; }
        public static AdLanguage CurrentLanguage { get; set; }
        public static JObject Resource { get; set; }
        public static string ResourceValue(string caption)
        {
            try
            {
                return Resource[caption].HasValues ? caption : Resource[caption].Value<string>();
            }
            catch (Exception)
            {
                return caption;
            }
        }

        public static SessionUserLogin GetSessionUser(this HttpContext context)
        {
            SessionUserLogin session = null;
            if (context.Session.IsExists("E_SESSION_USER"))
            {
                session = context.Session.Get<SessionUserLogin>("E_SESSION_USER");
            }
            return session;
        }

        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static void SetSessionUser(this HttpContext context, SessionUserLogin session)
        {
            context.Session.Set("E_SESSION_USER", session);
        }

        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static string GenerateOTP(int length = 4, bool onlyNumber = true)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            if (!onlyNumber)
            {
                characters += alphabets + small_alphabets + numbers;
            }
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character, StringComparison.Ordinal) != -1);
                otp += character;
            }

            return otp;
        }

        public static JMessage SendMail(string from, string to, string title, string content, string host, int port, string userName, string passWord)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from));
                message.To.Add(new MailboxAddress(to));
                message.Subject = title;
                message.Body = new TextPart
                {
                    Text = content,
                };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(host, port, SecureSocketOptions.Auto);
                    client.Authenticate(userName, passWord);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = ex.Message;
            }
            return msg;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int GetWeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string GetGoogleDistanceMatrixKey()
        {
            return GoogleDistanceMatrixKey;
        }

        public static byte[] GeneratorQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        //Call Procedure return DataTable
        public static DataTable GetDataTableProcedureSql(string storeName, string[] parameters, object[] values)
        {
            DataTable dt = new DataTable();
            string sql = storeName;
            using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                for (int index = 0; index < parameters.Length; ++index)
                {
                    sqlCmd.Parameters.AddWithValue(parameters[index], values[index]);
                }
                sqlConn.Open();
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                {
                    sqlAdapter.Fill(dt);
                }
                sqlConn.Close();
            }
            return dt;
        }


        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.UseDefaultCredentials = true;

            return webRequest;
        }
        public static async Task<JMessage> SendAPIRequest(string url)
        {

            var msg = new JMessage() { Error = false };
            try
            {
                HttpWebRequest request = CreateWebRequest(url);

                using (var response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (request.HaveResponse && response != null)
                    {
                        using (var rd = new StreamReader(response.GetResponseStream()))
                        {
                            msg.Object = rd.ReadToEnd();

                        }
                    }
                }
            }
            catch (WebException wex)
            {
                msg.Error = true;

            }

            return msg;
        }
    }

    public class SessionUserLogin
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EmployeeCode { get; set; }
        public string MainRole { get; set; }
        public string BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Picture { get; set; }
        public short UserType { get; set; }
        public List<PermissionObject> Permissions { get; set; }

        public int? AppId { get; set; }
        public string AppCode { get; set; }
        public string AppTitle { get; set; }
        public string AppIcon { get; set; }
        public string AppUrl { get; set; }

        public double SessionTimeOut { get; set; }
        public DateTime ExpireTimeSpan { get; set; }

        public string CompanyCode { get; set; }

        public bool HasPermission(string controllerName, string actionName)
        {
            bool isValid = false;

            var urlApi = controllerName + "_" + actionName;
            if (UserType == 10 || urlApi.Equals("Home_Permission") || urlApi.Equals("Home_SystemLocked") || urlApi.Equals("Language_Translation") || urlApi.Equals("Home_Logout"))
            {
                isValid = true;
            }
            else
            {
                if (Permissions.Count > 0 && !string.IsNullOrEmpty(urlApi))
                {
                    isValid = Permissions.Any(x => x.ResourceCode != null && x.ResourceCode.ToLower().Equals(urlApi.ToLower()));
                }
            }

            return isValid;
        }

        public PermissionObject GetPermission(string controllerName, string actionName)
        {
            var urlApi = controllerName + "_" + actionName;
            var permission = Permissions.FirstOrDefault(x => x.ResourceCode != null && x.ResourceCode.ToLower().Equals(urlApi.ToLower()));

            return permission;
        }
    }

    public class PermissionObject
    {
        public int? FunctionId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionTitle { get; set; }

        public int? ResourceId { get; set; }
        public string ResourceCode { get; set; }
        public string ResourceTitle { get; set; }
        public string ResourceApi { get; set; }

        public string GroupUserCode { get; set; }
        public string GroupUserTitle { get; set; }

        public string RoleId { get; set; }
        public string RoleTitle { get; set; }
    }
}
