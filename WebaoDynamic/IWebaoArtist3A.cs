using System.Collections.Generic;
using Webao.Attributes;
using Webao.Base;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    [AddParameter("api_key", LastFmAPI.API_KEY)]
    public interface IWebaoArtist3A
    {
        [Get("?method=artist.getinfo&artist={name}")]
        [Mapping(typeof(DtoArtist), With = "WebaoTestProject.Dto.GetArtist")]
        Artist GetInfo(string name);

        [Get("?method=artist.search&artist={name}&page={page}")]
        [Mapping(typeof(DtoSearch), With = "WebaoTestProject.Dto.DtoSearch.GetArtistsList")]
        List<Artist> Search(string name, int page);
    }
}