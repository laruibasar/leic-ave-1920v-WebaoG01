using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using Webao.Base;
using WebaoDynamic;
using WebaoDynamic.Part3;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicLazyTest
    {
        static readonly ServiceTracks serviceTracks = new ServiceTracks(new HttpRequestLazy(20, 10));

        [Test]
        public void TestServiceTracks()
        {
            Assert.IsNotNull(serviceTracks);
        }
    }
}
