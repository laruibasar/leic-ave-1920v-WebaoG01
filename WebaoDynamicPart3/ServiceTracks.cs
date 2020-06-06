using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamicPart3
{
    class ServiceTracks  : AbstractAccessObject
    {
        private readonly WebaoTrack webao;
        public ServiceTracks() : this(new HttpRequest()) { 

        }
        public ServiceTracks(IRequest req) : base(req) { }
        public IEnumerable<Track> TopTracksFrom(string country) {
           List<Track> list = (List<Track>)Request(country);
            foreach (Track item in list)
            {
                yield return item;
            }
        }
    }
}
