using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy  
{
    public delegate List<Artist> MyDelegate();

	public class WebaoArtistDummy3b : WebaoDyn, IWebaoArtist3
	{		
		public WebaoArtistDummy3b(IRequest req) : base(req)
		{
			base.SetUrl("http://ws.audioscrobbler.com/2.0/");
			base.SetParameter("format", "json");
			base.SetParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
		}

		public Artist GetInfo(string name)
		{
			string path = "?method=artist.getinfo&artist={name}";
			path = path.Replace("{name}", name.ToString());

			DtoArtist results = (DtoArtist)base.GetRequest(path, typeof(DtoArtist));
			Func<DtoArtist, Artist> Del = dto => dto.Artist;

			Artist artist = (Artist)Del.DynamicInvoke(results);

			return artist;
		}

		public List<Artist> Search(string name, int page)
		{
			string path = "?method=artist.getinfo&artist={name}";
			path = path.Replace("{name}", name.ToString());
			path = path.Replace("{page}", page.ToString());

			DtoSearch results = (DtoSearch)base.GetRequest(path, typeof(DtoSearch));
			
			Func<DtoSearch, List<Artist>> Del = dto => dto.Results.ArtistMatches.Artist; 
			
			List<Artist> list = (List<Artist>)Del.DynamicInvoke(results);

			return list;
		}
	}
}
