﻿using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy  
{
    public delegate Artist MyArtistDelegate();
    public delegate List<Artist> MyListDelegate();

    public class WebaoArtistDummy3A : WebaoDyn, IWebaoArtist3A
    {
        public WebaoArtistDummy3A(IRequest req) : base(req)
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
            MyArtistDelegate artistDelegate = dto.GetArtist;

            return artistDelegate();
        }

        public List<Artist> Search(string name, int page)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());
            path = path.Replace("{page}", page.ToString());

            DtoSearch dto = (DtoSearch)base.GetRequest(path, typeof(DtoSearch));
            MyListDelegate listDelegate = dto.GetArtistsList;

            return listDelegate();
        }       
    }
} 