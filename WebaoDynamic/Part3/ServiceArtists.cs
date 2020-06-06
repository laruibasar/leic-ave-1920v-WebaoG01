using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamic.Part3
{
    class ServiceArtists 
    {
        private readonly IWebaoArtist webao;
        public ServiceArtists() : this(new HttpRequestLazy())
        {
        }
        public ServiceArtists(IRequest req)  {
            webao = (IWebaoArtist)WebaoDynBuilder.Build(typeof(IWebaoArtist), req);
        }
        //public IEnumerable<Artist> Search(string name)
        //{
        //    List<Artist> list = webao.Search(name);
        //    foreach (Artist item in list)
        //    {
        //        yield return item;
        //    }
        //}
        public IEnumerable<Artist> Search(string name, int page)
        {
            List<Artist> list = webao.Search(name, page);
            foreach (Artist item in list)
            {
                yield return item;
            }
        }
    }
}
