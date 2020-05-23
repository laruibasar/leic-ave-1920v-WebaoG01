using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
using WebaoDynDummy;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest3b 
    {
        static readonly IWebaoArtist3b webaoArtist = (IWebaoArtist3b)WebaoDynBuilder3b
            .For<WebaoDynArtist>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", "************")
            .On("GetInfo")
            .GetFrom("?method=artist.getinfo&artist={name}")
            .Mapping<DtoArtist>(dto => dto.Artist)
            .On("Search")
            .GetFrom("?method=artist.search&artist={name}&page={page}")
            .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
            .Build3b(new HttpRequest());

    }
}
