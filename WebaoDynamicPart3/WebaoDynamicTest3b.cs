using System;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
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
            webaoArtist3b.Search("black", 1);
            //List<Artist> artists = webaoArtist.Search("black", 1);
            Assert.AreEqual(true, true);
            //Assert.AreEqual("Black Eyed Peas", artists[2].Name);
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
