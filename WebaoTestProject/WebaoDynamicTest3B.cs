using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
using WebaoDynamic.TP3Fluent;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest3B
    {
        public readonly WebaoArtist webaoArtist3b = (WebaoArtist)WebaoDynBuilder
            .For<WebaoArtist>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", "************")
            .On("GetInfo")
            .GetFrom("?method=artist.getinfo&artist={name}")
            .Mapping<DtoArtist>(dto => dto.Artist)
            .On("Search")
            .GetFrom("?method=artist.search&artist={name}&page={page}")
            .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)
            .Build3b(new HttpRequest());

        public readonly Context context = ContextCache.Get(typeof(WebaoArtist));
        public readonly DtoSearch results = new DtoSearch
        {
            Results = new DtoResults
            {
                ArtistMatches = new DtoArtistMatches
                {
                    Artist = new List<Artist>() {
                        new Artist { Name = "Thievery Corporation" },
                        new Artist { Name = "Pink Floyd" },
                        new Artist { Name = "Redkone" }
                    }
                }
            }
        };

        public readonly List<Artist> list = (List<Artist>)ContextCache.Get(typeof(WebaoArtist)).info.list[1].Del.DynamicInvoke(new DtoSearch
        {
            Results = new DtoResults
            {
                ArtistMatches = new DtoArtistMatches
                {
                    Artist = new List<Artist>() { new Artist { Name = "xpto artist" } }
                }
            }
        });

        [Test]
        public void TestUseContext()
        {
            Assert.AreEqual(context.info.url, "http://ws.audioscrobbler.com/2.0/");
            Assert.AreEqual(context.info.list[0].name, "GetInfo");
            Assert.AreEqual(context.info.list[1].name, "Search");
            Assert.AreEqual(list[0].Name, "xpto artist");
        }

        [Test]
        public void TestUseDelegateFromContext()
        {
            List<Artist> artists = (List<Artist>)context.info.list[1].Del.DynamicInvoke(results);
            Assert.AreEqual("Thievery Corporation", artists[0].Name);
            Assert.AreEqual("Pink Floyd", artists[1].Name);
            Assert.AreEqual("Redkone", artists[2].Name);
        }     
/* TestUseDelegateFromContext() cil managed
  {
    .custom instance void [nunit.framework]NUnit.Framework.TestAttribute::.ctor() = ( 01 00 00 00 )
    // Code size       120 (0x78)
    .maxstack  5
    .locals init ([mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist> V_0)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class WebaoDynamic.TP3Fluent.Context WebaoTestProject.WebaoDynamicTest3B::context
    IL_0006:  ldfld      class WebaoDynamic.TP3Fluent.Info WebaoDynamic.TP3Fluent.Context::info
    IL_000b:  ldfld      [mscorlib]System.Collections.Generic.List`1<class WebaoDynamic.TP3Fluent.InfoMethod> WebaoDynamic.TP3Fluent.Info::list
    IL_0010:  ldc.i4.1
    IL_0011:  callvirt   instance !0 [mscorlib]System.Collections.Generic.List`1<class WebaoDynamic.TP3Fluent.InfoMethod>::get_Item(int32)
    IL_0016:  callvirt   instance [mscorlib]System.Delegate WebaoDynamic.TP3Fluent.InfoMethod::get_Del()
    IL_001b:  ldc.i4.1
    IL_001c:  newarr     [mscorlib]System.Object
    IL_0021:  dup
    IL_0022:  ldc.i4.0
    IL_0023:  ldarg.0
    IL_0024:  ldfld      class WebaoTestProject.Dto.DtoSearch WebaoTestProject.WebaoDynamicTest3B::results
    IL_0029:  stelem.ref
    IL_002a:  callvirt   instance object [mscorlib]System.Delegate::DynamicInvoke(object[])
    IL_002f:  castclass  [mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>
    IL_0034:  stloc.0
    IL_0035:  ldstr      "Thievery Corporation"
    IL_003a:  ldloc.0
    IL_003b:  ldc.i4.0
    IL_003c:  callvirt   instance !0 [mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>::get_Item(int32)
    IL_0041:  callvirt   instance string WebaoTestProject.Dto.Artist::get_Name()
    IL_0046:  call       void [nunit.framework]NUnit.Framework.Assert::AreEqual(object,
                                                                                object)
    IL_004b:  ldstr      "Pink Floyd"
    IL_0050:  ldloc.0
    IL_0051:  ldc.i4.1
    IL_0052:  callvirt   instance !0 [mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>::get_Item(int32)
    IL_0057:  callvirt   instance string WebaoTestProject.Dto.Artist::get_Name()
    IL_005c:  call       void [nunit.framework]NUnit.Framework.Assert::AreEqual(object,
                                                                                object)
    IL_0061:  ldstr      "Redkone"
    IL_0066:  ldloc.0
    IL_0067:  ldc.i4.2
    IL_0068:  callvirt   instance !0 [mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>::get_Item(int32)
    IL_006d:  callvirt   instance string WebaoTestProject.Dto.Artist::get_Name()
    IL_0072:  call       void [nunit.framework]NUnit.Framework.Assert::AreEqual(object,
                                                                                object)
    IL_0077:  ret
*/
        [Test]
        public void TestLoadAndUseContext()
        {
            Context ct = ContextCache.Get(typeof(WebaoArtist));
            List<InfoMethod> methods = ct.info.list;
            foreach (InfoMethod method in methods)
            {
                if (method.name.Equals("Search"))
                {
                    List<Artist> artists = (List<Artist>)method.Del.DynamicInvoke(results);
                    Assert.AreEqual("Pink Floyd", artists[1].Name);
                }
            }
        }
        /* IL generated 
.method public hidebysig instance void
    TestLoadAndUseContext() cil managed
{
    .custom instance void[nunit.framework] NUnit.Framework.TestAttribute::.ctor() = ( 01 00 00 00 )
    // Code size       139 (0x8b)
    .maxstack  5
    .locals init([mscorlib] System.Collections.Generic.List`1/Enumerator<class WebaoDynamic.TP3Fluent.InfoMethod> V_0,
             class WebaoDynamic.TP3Fluent.InfoMethod V_1,
             [mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist> V_2)
    IL_0000:  ldtoken WebaoTestProject.WebaoArtist
    IL_0005:  call[mscorlib] System.Type[mscorlib] System.Type::GetTypeFromHandle([mscorlib] System.RuntimeTypeHandle)
    IL_000a:  call class WebaoDynamic.TP3Fluent.Context WebaoDynamic.TP3Fluent.ContextCache::Get([mscorlib] System.Type)
    IL_000f:  ldfld class WebaoDynamic.TP3Fluent.Info WebaoDynamic.TP3Fluent.Context::info

    IL_0014:  ldfld[mscorlib] System.Collections.Generic.List`1<class WebaoDynamic.TP3Fluent.InfoMethod> WebaoDynamic.TP3Fluent.Info::list
    IL_0019:  callvirt instance[mscorlib]System.Collections.Generic.List`1/Enumerator<!0>[mscorlib] System.Collections.Generic.List`1<class WebaoDynamic.TP3Fluent.InfoMethod>::GetEnumerator()
    IL_001e:  stloc.0
    .try
    {
      IL_001f:  br.s IL_0071

      IL_0021:  ldloca.s V_0
      IL_0023:  call instance !0 [mscorlib] System.Collections.Generic.List`1/Enumerator<class WebaoDynamic.TP3Fluent.InfoMethod>::get_Current()
      IL_0028:  stloc.1
      IL_0029:  ldloc.1
      IL_002a:  callvirt instance string WebaoDynamic.TP3Fluent.InfoMethod::get_name()
      IL_002f:  ldstr      "Search"
      IL_0034:  callvirt instance bool[mscorlib] System.String::Equals(string)
      IL_0039:  brfalse.s IL_0071

      IL_003b:  ldloc.1
      IL_003c:  callvirt instance[mscorlib]System.Delegate WebaoDynamic.TP3Fluent.InfoMethod::get_Del()
      IL_0041:  ldc.i4.1
      IL_0042:  newarr[mscorlib] System.Object
      IL_0047:  dup
      IL_0048:  ldc.i4.0
      IL_0049:  ldarg.0
      IL_004a:  ldfld      class WebaoTestProject.Dto.DtoSearch WebaoTestProject.WebaoDynamicTest3B::results
      IL_004f:  stelem.ref
      IL_0050:  callvirt instance object[mscorlib] System.Delegate::DynamicInvoke(object[])
      IL_0055:  castclass[mscorlib] System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>
      IL_005a:  stloc.2
      IL_005b:  ldstr      "Pink Floyd"
      IL_0060:  ldloc.2
      IL_0061:  ldc.i4.1
      IL_0062:  callvirt instance !0 [mscorlib] System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist>::get_Item(int32)
      IL_0067:  callvirt instance string WebaoTestProject.Dto.Artist::get_Name()
      IL_006c:  call void[nunit.framework] NUnit.Framework.Assert::AreEqual(object,
                                                                            object)
      IL_0071:  ldloca.s V_0
      IL_0073:  call instance bool[mscorlib] System.Collections.Generic.List`1/Enumerator<class WebaoDynamic.TP3Fluent.InfoMethod>::MoveNext()
      IL_0078:  brtrue.s IL_0021

      IL_007a:  leave.s IL_008a

      }  // end .try
    finally
    {
        IL_007c:  ldloca.s V_0
        IL_007e:  constrained. [mscorlib] System.Collections.Generic.List`1/Enumerator<class WebaoDynamic.TP3Fluent.InfoMethod>
        IL_0084:  callvirt instance void[mscorlib] System.IDisposable::Dispose()
        IL_0089:  endfinally
    }  // end handler
        IL_008a:  ret
    } // end of method WebaoDynamicTest3B::TestLoadAndUseContext
    */

    }
}
