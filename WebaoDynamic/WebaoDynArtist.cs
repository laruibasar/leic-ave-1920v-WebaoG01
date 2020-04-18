﻿using System;
using Webao;
using Webao.Attributes;
using Webao.Base;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    [AddParameter("api_key", "************")]
    public interface WebaoDynArtist
    {
        [Get("?method=artist.getinfo&artist={name}")]
        [Mapping(typeof(DtoArtist), ".Artist")]
        Artist GetInfo(string name);


        [Get("?method=artist.search&artist={name}&page={page}")]
        [Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
        List<Artist> Search(string name, int page);
    }
}