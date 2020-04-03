using System.Collections.Generic;
using Webao;
using Webao.Attributes;
using Webao.Base;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    [AddParameter("api_key", LastFmAPI.API_KEY)]
    public class WebaoArtist : AbstractAccessObject
    {
        public WebaoArtist(IRequest req) : base(req) { }
       
        [Get("?method=artist.getinfo&artist={name}")]
        [Mapping(typeof(DtoArtist), ".Artist")]
        public Artist GetInfo(string name) => (Artist)Request(name); 
         

        [Get("?method=artist.search&artist={name}")]
        [Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
        public List<Artist> Search(string name) => (List<Artist>)Request(name);       
    }
}