using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using XS156Client35.Models;

namespace XS156Client35.Helper
{
    [ComVisible(false)]
    public class Connections<T>
    {
        public static object Get(string connectionUri)
        {
           
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var response = client.DownloadString(connectionUri);
                try
                {
                    return (JsonConvert.DeserializeObject<T>(response));
                }
                catch (Exception)
                {
                    return (JsonConvert.DeserializeObject<IEnumerable<T>>(response));
                }
            }
        }

        public static object GetList(string uri)
        {

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var response = client.DownloadString(uri);
                return (JsonConvert.DeserializeObject<IEnumerable<T>>(response));
            }
        }
        public static object Put(string uri, object data)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serialisedData = JsonConvert.SerializeObject(data);
                var response = client.UploadString(uri,"PUT" ,serialisedData);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }
       
        public static object Put(string uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serialisedData = JsonConvert.SerializeObject("");
                var response = client.UploadString(uri, "PUT", serialisedData);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }

        public static object Post(string uri, object data)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serialisedData = JsonConvert.SerializeObject(data);
                var response = client.UploadString(uri, "POST", serialisedData);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }

        public static object Post(string uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serialisedData = JsonConvert.SerializeObject("");
                var response = client.UploadString(uri, "POST", serialisedData);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }

        public static object Delete(string uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string serialisedData = JsonConvert.SerializeObject("");
                var response = client.UploadString(uri, "DELETE", serialisedData);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }
        
    }

}
