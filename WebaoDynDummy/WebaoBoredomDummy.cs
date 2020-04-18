using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Webao;
using Webao.Attributes;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    //[BaseUrl("https://www.boredapi.com/api/")]
    [AddParameter("format", "json")]
    public class WebaoBoredomDummy : WebaoDynBoredom
    {
        private readonly IRequest req;
        private readonly Dictionary<string, string> requestParameters;
        private readonly Regex regex = new Regex(@"\{[a-zA-Z0-9]*\}");
        private readonly char[] separator = new char[] { '.' };

        public WebaoBoredomDummy(IRequest req) 
        {
            this.req = req;

            req.BaseUrl(WebaoOps.GetUrl(typeof(WebaoDynBoredom)));
            requestParameters = WebaoOps.GetParameters(typeof(WebaoBoredomDummy));

            foreach (KeyValuePair<string, string> pair in requestParameters)
            {
                req.AddParameter(pair.Key, pair.Value);
            }
        }

        public Boredom GetActivityByKey(int key)
        {
            /* Build query string, with path from custom attributes */
            string path = WebaoOps.GetQuery(typeof(WebaoDynBoredom), "GetActivityByKey");
            path = regex.Replace(path, key.ToString());

            /* Make request */
            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynBoredom), "GetActivityByKey");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            /*
             * Get object from properties from response
             */
            Type type = obj.GetType();
            PropertyInfo prop;

            /*
             * We use iteration and then trash objects as needed
             * as we dive deeper into the domain.
             * We rely on GC to clean lost references.
             */
            object newObj = new object();
            foreach (string domain in domains)
            {
                prop = type.GetProperty(domain);
                newObj = prop.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }

            return (Boredom)obj;
        }

        public Boredom GetActivity(int participants, float price)
        {
            /* Build query string, with path from custom attributes */
            string path = WebaoOps.GetQuery(typeof(WebaoDynBoredom), "GetActivity");
            path = path.Replace("{participants}", participants.ToString());
            path = path.Replace("{price}", price.ToString());

            /* Make request */
            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynBoredom), "GetActivity");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            /*
             * Get object from properties from response
             */
            Type type = obj.GetType();
            PropertyInfo prop;

            /*
             * We use iteration and then trash objects as needed
             * as we dive deeper into the domain.
             * We rely on GC to clean lost references.
             */
            object newObj = new object();
            foreach (string domain in domains)
            {
                prop = type.GetProperty(domain);
                newObj = prop.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }

            return (Boredom)obj;
        }
    }
}
