using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using Webao.Base;
using WebaoDynamic;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamicPart3
{
    public class ServiceTracks 
    {
        private readonly WebaoTrack webao;
        private static int elemCount;
        public ServiceTracks() : this(new HttpRequestLazy()) { }
        
        public ServiceTracks(IRequest req)  {
  //          req.BaseUrl("http://ws.audioscrobbler.com/2.0/");
  //          req.AddParameter("format", "json");
  //          req.AddParameter("api_key", LastFmAPI.API_KEY);
            webao = (WebaoTrack)WebaoDynBuilder.Build(typeof(WebaoTrack), req);
        }

        public IEnumerable<Track> TopTracksFrom(string country) {
            List<Track> list = webao.GeoGetTopTracks(country);
           
            foreach (Track item in list)
            {
                yield return item;
            }
        }
    }
}
