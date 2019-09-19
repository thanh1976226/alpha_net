using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ESEIM.Utils
{
    public interface IFCMPushNotification
    {
        JMessage SendNotification(string _title, string _message, List<String> devices, Object dataObject);
    }
    public class FCMPushNotification : IFCMPushNotification
    {
        public JMessage SendNotification(string _title, string _message, List<string> devices, Object dataObject)
        {
            var msg = new JMessage { Error = false, Title = "" };
            var API_KEY = "AAAAc3fLTi0:APA91bG3aEGgAenV6jbF8CU-mSjLKyWOkRal9GvMW1AFfscBfXO9RK79ikM-2FCiflxSHI5cRECoMgA0AbP2cTEsSPiB__HLncOYH6vRekCW5YUI-fZmjQawEx-QdqBZ4WDQ9-AxiuUY";
            var senderId = "495931051565";
            HttpRequestMessage httpRequest = null;
            HttpClient httpClient = null;
            try
            {
                var dataMessage = new
                {
                    registration_ids = devices,
                    data = new
                    {
                        title = _title,
                        body = _message,
                        sound = "default",
                        click_action = "OPEN_ACTIVITY_MAIN",
                        //content_available = true,
                        //priority = "high"
                    },
                    priority = "high",
                    content_available = true
                    //data = dataObject
                };
                httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
                var json = JsonConvert.SerializeObject(dataMessage);
                httpRequest.Headers.TryAddWithoutValidation("Authorization", "key=" + API_KEY);
                httpRequest.Headers.TryAddWithoutValidation("Sender", $"id={senderId}");
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        msg.Error = false;
                        msg.Object = response.Content;
                    }
                    else
                    {
                        msg.Error = true;
                        msg.Object = response.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;

            }
            finally
            {
                httpRequest.Dispose();
                httpClient.Dispose();
            }
            return msg;
        }
    }
}
