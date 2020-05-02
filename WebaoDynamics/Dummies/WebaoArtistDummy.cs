using System.Collections.Generic;
using Webao;
using Webao.Dto;
using WebaoDynamics.Interfaces;

namespace WebaoDynamics.Dummies  
{
    public class WebaoArtistDummy : WebaoDyn, IWebaoArtist
    {
        public WebaoArtistDummy(IRequest req) : base(req)
        {
            base.SetUrl("http://ws.audioscrobbler.com/2.0/");
            base.SetParameter("format", "json");
            base.SetParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
        }

        public Artist GetInfo(string name)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());

            DtoArtist dto = (DtoArtist)base.GetRequest(path, typeof(DtoArtist));

            return dto.Artist;
        } 

        public List<Artist> Search(string name, int page)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());
            path = path.Replace("{page}", page.ToString());

            DtoSearch dto = (DtoSearch)base.GetRequest(path, typeof(DtoSearch));

            return dto.Results.ArtistMatches.Artist;
        }
    }
}
