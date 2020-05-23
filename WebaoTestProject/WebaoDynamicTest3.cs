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
    public class WebaoDynamicTest3  
    {
        WebaoArtistDummy3 WebaoArtistDummy3 = new WebaoArtistDummy3(new HttpRequest());


        static readonly IWebaoArtist3 webaoArtist = (IWebaoArtist3)WebaoDynBuilder3.Build(typeof(IWebaoArtist3), new HttpRequest());
        //static readonly IWebaoArtist webaoArtistMock = (IWebaoArtist)WebaoDynBuilder3.Build(typeof(IWebaoArtist3), new LastfmMockRequest());

        [Test] 
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = webaoArtist.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        //[Test]
        //public void TestWebaoArtistSearchMock()
        //{
        //    List<Artist> artists = webaoArtistMock.Search("black", 1);
        //    Assert.AreEqual("Black Sabbath", artists[1].Name);
        //    Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        //}

    }
}