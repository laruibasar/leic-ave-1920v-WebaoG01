using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using Webao.Base;
using WebaoDynamic;
using WebaoDynamic.TP3Fluent;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest3B
    {


        static readonly WebaoDynArtist webaoArtist = (WebaoDynArtist)WebaoDynBuilder
            .For<WebaoDynArtist>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", LastFmAPI.API_KEY)
            .On("GetInfo")
            .GetFrom("?method=artist.getinfo&artist={name}")
            .Mapping<DtoArtist>(dto => dto.Artist)
            .On("Search")
            .GetFrom("?method=artist.search&artist={name}&page={page}")
            .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
            .Build(new HttpRequest());

        [Test]
        public void TestArtistSearch()
        {
            List<Artist> listArtists = webaoArtist.Search("black", 1);
            Assert.AreEqual("Black Sabbath", listArtists[1].Name);
            Assert.AreEqual("Black Eyed Peas", listArtists[2].Name);
        }

        //static readonly WebaoArtist webaoArtist3b = (WebaoArtist)WebaoDynBuilder
        //    .For<WebaoArtist>("http://ws.audioscrobbler.com/2.0/")
        //    .AddParameter("format", "json")
        //    .AddParameter("api_key", LastFmAPI.API_KEY)
        //    .On("GetInfo")
        //    .GetFrom("?method=artist.getinfo&artist={name}")
        //    .Mapping<DtoArtist>(dto => dto.Artist)
        //    .On("Search")
        //    .GetFrom("?method=artist.search&artist={name}&page={page}")
        //    .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
        //    .Build(new HttpRequest());

        //static readonly Context context = ContextCache.Get(typeof(WebaoArtist));
        //static readonly DtoSearch results = new DtoSearch
        //{
        //    Results = new DtoResults
        //    {
        //        ArtistMatches = new DtoArtistMatches
        //        {
        //            Artist = new List<Artist>() {
        //                new Artist { Name = "Thievery Corporation" },
        //                new Artist { Name = "Pink Floyd" },
        //                new Artist { Name = "Redkone" }
        //            }
        //        }
        //    }
        //};

        //static readonly List<Artist> list = (List<Artist>)ContextCache.Get(typeof(WebaoArtist)).info.list[1].Del.DynamicInvoke(new DtoSearch
        //{
        //    Results = new DtoResults
        //    {
        //        ArtistMatches = new DtoArtistMatches
        //        {
        //            Artist = new List<Artist>() { new Artist { Name = "xpto artist" } }
        //        }
        //    }
        //});

        //[Test]
        //public void TestUseContext()
        //{
        //    Assert.AreEqual(context.info.url, "http://ws.audioscrobbler.com/2.0/");
        //    Assert.AreEqual(context.info.list[0].name, "GetInfo");
        //    Assert.AreEqual(context.info.list[1].name, "Search");
        //    Assert.AreEqual(list[0].Name, "xpto artist");
        //}

        //[Test]
        //public void TestUseDelegateFromContext()
        //{
        //    List<Artist> artists = (List<Artist>)context.info.list[1].Del.DynamicInvoke(results);
        //    Assert.AreEqual("Thievery Corporation", artists[0].Name);
        //    Assert.AreEqual("Pink Floyd", artists[1].Name);
        //    Assert.AreEqual("Redkone", artists[2].Name);
        //}     

        //[Test]
        //public void TestLoadAndUseContext()
        //{
        //    List<Artist> artists = (List<Artist>)ContextCache.Get(typeof(WebaoArtist))
        //        .info
        //        .GetMethodDelegate("Search")
        //        .DynamicInvoke(results);

        //    Assert.AreEqual("Pink Floyd", artists[1].Name);
        //}
    }
}
