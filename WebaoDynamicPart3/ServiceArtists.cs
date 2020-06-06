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
    class ServiceArtists : AbstractAccessObject
    {
        private readonly WebaoArtist webao;
        public ServiceArtists() : this(new HttpRequest())
        {
        }
        public ServiceArtists(IRequest req) :base(req) { }
        public IEnumerable<Artist> Search(string name)
        {
            List<Artist> list = (List<Artist>)Request(name);
            foreach (Artist item in list)
            {
                yield return item;
            }
        }
        public IEnumerable<Artist> Search(string name, int page)
        {
            List<Artist> list = (List<Artist>)Request(name, page);
            foreach (Artist item in list)
            {
                yield return item;
            }
        }
    }
}
