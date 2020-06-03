using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy  
{
    public delegate List<Artist> MyDelegate();

    public class WebaoArtistDummy3 : WebaoDyn, IWebaoArtist3
    {
        public WebaoArtistDummy3(IRequest req) : base(req)
        {
            base.SetUrl("http://ws.audioscrobbler.com/2.0/");
            base.SetParameter("format", "json");
            base.SetParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
        }

        public List<Artist> Search(string name, int page)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());
            path = path.Replace("{page}", page.ToString());

            DtoSearch dto = (DtoSearch)base.GetRequest(path, typeof(DtoSearch)); 

            MyDelegate myDelegate = dto.GetArtistsList;
            //Func<List<Artist>> myDelegate2 = dto.GetArtistsList;

            //return dto.Results.ArtistMatches.Artist;
            return myDelegate();
        }       
    }
} 
