using System;
using System.Collections.Generic;
using Webao;

namespace WebaoDynamicsDelegates
{
    public class WebaoDyn
    {
        private readonly IRequest req;
        protected readonly Dictionary<string, string> parameters = new Dictionary<string, string>();

        public WebaoDyn(IRequest req)
        {
            this.req = req;
        }

        public void SetUrl(string url)
        {
            req.BaseUrl(url);
        }

        public void SetParameter(string key, string value)
        {
            req.AddParameter(key, value);
        }

        public object GetRequest(string path, Type requestType)
        {
            return req.Get(path, requestType);
        }
    }
}
