using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Webao
{
    public class HttpRequestLazy : HttpRequest
    {
        private static readonly HttpClient client = new HttpClient();

        public HttpRequestLazy() : base()
        {
            this.Page = 0;
            this.Limit = 20;
        }

        public HttpRequestLazy(int limit) : base()
        {
            this.Page = 0;
            this.Limit = limit;
        }

        public override int Page { get; set; }

        public override int Limit { get; set; }

        public new string Url(string path)
        {
            string url = base.Url(path);
            return url + "&limit=" + Limit.ToString() + "&page=" + Page.ToString();
        }

        public override object Get(string path, Type targetType)
        {
            Page++;

            path = path.Replace(",", ".");
            string body = client
                            .GetStringAsync(Url(path))
                            .Result;
            return JsonConvert.DeserializeObject(body, targetType);
        }

        public int GetNumberRequest()
        {
            return Page;
        }
    }
}
