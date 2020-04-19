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
    //[BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    //[AddParameter("api_key", LastFmAPI.API_KEY)]
    public class WebaoTrackDummy : WebaoDynTrack
    {
        private readonly IRequest req;
        private readonly Dictionary<string, string> requestParameters;
        private readonly char[] separator = new char[] { '.' };

        public WebaoTrackDummy(IRequest req)
        {
            this.req = req;

            req.BaseUrl(WebaoOps.GetUrl(typeof(WebaoDynTrack)));
            requestParameters = WebaoOps.GetParameters(typeof(WebaoTrackDummy));

            foreach (KeyValuePair<string, string> pair in requestParameters)
            {
                req.AddParameter(pair.Key, pair.Value);
            }
        }

        public List<Track> GeoGetTopTracks(string country)
        {
            /* Build query string, with path from custom attributes */
            string path = WebaoOps.GetQuery(typeof(WebaoDynTrack), "GeoGetTopTracks");
            path = path.Replace("{country}", country);

            /* Make request */
            MappingAttribute map = WebaoOps.GetMapping(typeof(WebaoDynTrack), "GeoGetTopTracks");
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

            return (List<Track>)obj;
        }
    }
}
