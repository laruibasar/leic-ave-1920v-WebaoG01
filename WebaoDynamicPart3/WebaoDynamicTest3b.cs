using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
using WebaoDynamicPart3;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest3b   
    {
        IWebaoArtist3b webaoArtist3b = (IWebaoArtist3b)WebaoDynBuilder3b
            .For<IWebaoArtist3b>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", "************")
            .On("GetInfo")
            .GetFrom("?method=artist.getinfo&artist={name}")
            .Mapping<DtoArtist>(dto => dto.Artist)
            .On("Search")
            .GetFrom("?method=artist.search&artist={name}&page={page}")
            .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
            .Build3b(new HttpRequest());



        [Test]
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = webaoArtist3b.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        //static readonly IWebaoArtist3b webaoArtist = (IWebaoArtist3b)WebaoDynBuilder3b
        //    .For<WebaoDynArtist>("http://ws.audioscrobbler.com/2.0/")
        //    .AddParameter("format", "json")
        //    .AddParameter("api_key", "************")
        //    .On("GetInfo")
        //    .GetFrom("?method=artist.getinfo&artist={name}")
        //    .Mapping<DtoArtist>(dto => dto.Artist)
        //    .On("Search")
        //    .GetFrom("?method=artist.search&artist={name}&page={page}")
        //    .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
        //    .Build3b(new HttpRequest());

    }

    
}
