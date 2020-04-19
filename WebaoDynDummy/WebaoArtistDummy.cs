using System;
using System.Collections.Generic;
using System.Reflection;
using Webao;
using Webao.Attributes;
using Webao.Base;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy  
{
    [AddParameter("format", "json")]
    [AddParameter("api_key", LastFmAPI.API_KEY)]
    public class WebaoArtistDummy : WebaoDynArtist
    {
        private readonly IRequest req;
        private readonly Dictionary<string, string> requestParameters;
        private readonly char[] separator = new char[] { '.' };


        public WebaoArtistDummy(IRequest req)
        {
            this.req = req;

            req.BaseUrl(WebaoOps.GetUrl(typeof(WebaoDynArtist)));
            requestParameters = WebaoOps.GetParameters(typeof(WebaoArtistDummy));

            foreach (KeyValuePair<string, string> pair in requestParameters)
            {
                req.AddParameter(pair.Key, pair.Value);
            }
        }

        public Artist GetInfo(string name)
        {
            string path = WebaoOps.GetQuery(typeof(WebaoDynArtist), "GetInfo");
            path = path.Replace("{name}", name.ToString());

            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynArtist), "GetInfo");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            Type type = obj.GetType();
            PropertyInfo propInfo;

            object newObj = new object();
            foreach (string domain in domains)
            {
                propInfo = type.GetProperty(domain);
                newObj = propInfo.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }
            return (Artist)obj;
        } 

        public List<Artist> Search(string name, int page)
        {
            string path = WebaoOps.GetQuery(typeof(WebaoDynArtist), "Search");
            path = path.Replace("{name}", name.ToString());  
            path = path.Replace("{page}", page.ToString());   

            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynArtist), "Search");
            string[] domains = map.path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            object obj = req.Get(path, map.destType);

            Type type = obj.GetType(); 
            PropertyInfo propInfo;

            object newObj = new object();
            foreach (string domain in domains)
            { 
                propInfo = type.GetProperty(domain);
                newObj = propInfo.GetValue(obj);
                type = newObj.GetType();
                obj = newObj;
            }
            return (List<Artist>)obj;
        }
    }
}
