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

namespace WebaoDynamic.Part3
{
    public class ServiceTracks 
    {
        private readonly IWebaoTrack webao;
        private int elemCount;

        public ServiceTracks() : this(new HttpRequestLazy()) { }
        
        public ServiceTracks(IRequest req)  {
            //webao = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), req);
            webao = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), req);
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
